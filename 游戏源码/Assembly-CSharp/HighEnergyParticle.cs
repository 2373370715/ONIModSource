using System;
using KSerialization;
using UnityEngine;

// Token: 0x020013E5 RID: 5093
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticle : StateMachineComponent<HighEnergyParticle.StatesInstance>
{
	// Token: 0x06006892 RID: 26770 RVA: 0x000E48A4 File Offset: 0x000E2AA4
	protected override void OnPrefabInit()
	{
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Radbolt_travel_LP", false);
		base.OnPrefabInit();
	}

	// Token: 0x06006893 RID: 26771 RVA: 0x002D76D0 File Offset: 0x002D58D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticles.Add(this);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.HighEnergyParticleCount, base.gameObject);
		this.emitter.SetEmitting(false);
		this.emitter.Refresh();
		this.SetDirection(this.direction);
		base.gameObject.layer = LayerMask.NameToLayer("PlaceWithDepth");
		this.StartLoopingSound();
		base.smi.StartSM();
	}

	// Token: 0x06006894 RID: 26772 RVA: 0x002D7758 File Offset: 0x002D5958
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.StopLoopingSound();
		Components.HighEnergyParticles.Remove(this);
		if (this.capturedBy != null && this.capturedBy.currentParticle == this)
		{
			this.capturedBy.currentParticle = null;
		}
	}

	// Token: 0x06006895 RID: 26773 RVA: 0x002D77AC File Offset: 0x002D59AC
	public void SetDirection(EightDirection direction)
	{
		this.direction = direction;
		float angle = EightDirectionUtil.GetAngle(direction);
		base.smi.master.transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}

	// Token: 0x06006896 RID: 26774 RVA: 0x002D77EC File Offset: 0x002D59EC
	public void Collide(HighEnergyParticle.CollisionType collisionType)
	{
		this.collision = collisionType;
		GameObject gameObject = new GameObject("HEPcollideFX");
		gameObject.SetActive(false);
		gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.PosToCell(base.smi.master.transform.position), Grid.SceneLayer.FXFront));
		KBatchedAnimController fxAnim = gameObject.AddComponent<KBatchedAnimController>();
		fxAnim.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("hep_impact_kanim")
		};
		fxAnim.initialAnim = "graze";
		gameObject.SetActive(true);
		switch (collisionType)
		{
		case HighEnergyParticle.CollisionType.Captured:
			fxAnim.Play("full", KAnim.PlayMode.Once, 1f, 0f);
			break;
		case HighEnergyParticle.CollisionType.CaptureAndRelease:
			fxAnim.Play("partial", KAnim.PlayMode.Once, 1f, 0f);
			break;
		case HighEnergyParticle.CollisionType.PassThrough:
			fxAnim.Play("graze", KAnim.PlayMode.Once, 1f, 0f);
			break;
		}
		fxAnim.onAnimComplete += delegate(HashedString arg)
		{
			Util.KDestroyGameObject(fxAnim);
		};
		if (collisionType == HighEnergyParticle.CollisionType.PassThrough)
		{
			this.collision = HighEnergyParticle.CollisionType.None;
			return;
		}
		base.smi.sm.destroySignal.Trigger(base.smi);
	}

	// Token: 0x06006897 RID: 26775 RVA: 0x000E48CE File Offset: 0x000E2ACE
	public void DestroyNow()
	{
		base.smi.sm.destroySimpleSignal.Trigger(base.smi);
	}

	// Token: 0x06006898 RID: 26776 RVA: 0x002D7948 File Offset: 0x002D5B48
	private void Capture(HighEnergyParticlePort input)
	{
		if (input.currentParticle != null)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Particle was backed up and caused an explosion!"
			});
			base.smi.sm.destroySignal.Trigger(base.smi);
			return;
		}
		this.capturedBy = input;
		input.currentParticle = this;
		input.Capture(this);
		if (input.currentParticle == this)
		{
			input.currentParticle = null;
			this.capturedBy = null;
			this.Collide(HighEnergyParticle.CollisionType.Captured);
			return;
		}
		this.capturedBy = null;
		this.Collide(HighEnergyParticle.CollisionType.CaptureAndRelease);
	}

	// Token: 0x06006899 RID: 26777 RVA: 0x000E48EB File Offset: 0x000E2AEB
	public void Uncapture()
	{
		if (this.capturedBy != null)
		{
			this.capturedBy.currentParticle = null;
		}
		this.capturedBy = null;
	}

	// Token: 0x0600689A RID: 26778 RVA: 0x002D79DC File Offset: 0x002D5BDC
	public void CheckCollision()
	{
		if (this.collision != HighEnergyParticle.CollisionType.None)
		{
			return;
		}
		int cell = Grid.PosToCell(base.smi.master.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			gameObject.GetComponent<Operational>();
			HighEnergyParticlePort component = gameObject.GetComponent<HighEnergyParticlePort>();
			if (component != null)
			{
				Vector2 pos = Grid.CellToPosCCC(component.GetHighEnergyParticleInputPortPosition(), Grid.SceneLayer.NoLayer);
				if (base.GetComponent<KCircleCollider2D>().Intersects(pos))
				{
					if (component.InputActive() && component.AllowCapture(this))
					{
						this.Capture(component);
						return;
					}
					this.Collide(HighEnergyParticle.CollisionType.PassThrough);
				}
			}
		}
		KCircleCollider2D component2 = base.GetComponent<KCircleCollider2D>();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		ListPool<ScenePartitionerEntry, HighEnergyParticle>.PooledList pooledList = ListPool<ScenePartitionerEntry, HighEnergyParticle>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(num - 1, num2 - 1, 3, 3, GameScenePartitioner.Instance.collisionLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
			HighEnergyParticle component3 = kcollider2D.gameObject.GetComponent<HighEnergyParticle>();
			if (!(component3 == null) && !(component3 == this) && component3.isCollideable)
			{
				bool flag = component2.Intersects(component3.transform.position);
				bool flag2 = kcollider2D.Intersects(base.transform.position);
				if (flag && flag2)
				{
					this.payload += component3.payload;
					component3.DestroyNow();
					this.Collide(HighEnergyParticle.CollisionType.HighEnergyParticle);
					return;
				}
			}
		}
		pooledList.Recycle();
		GameObject gameObject2 = Grid.Objects[cell, 3];
		if (gameObject2 != null)
		{
			ObjectLayerListItem objectLayerListItem = gameObject2.GetComponent<Pickupable>().objectLayerListItem;
			while (objectLayerListItem != null)
			{
				GameObject gameObject3 = objectLayerListItem.gameObject;
				objectLayerListItem = objectLayerListItem.nextItem;
				if (!(gameObject3 == null))
				{
					KPrefabID component4 = gameObject3.GetComponent<KPrefabID>();
					Health component5 = gameObject2.GetComponent<Health>();
					if (component5 != null && component4 != null && component4.HasTag(GameTags.Creature) && !component5.IsDefeated())
					{
						component5.Damage(20f);
						this.Collide(HighEnergyParticle.CollisionType.Creature);
						return;
					}
				}
			}
		}
		GameObject gameObject4 = Grid.Objects[cell, 0];
		if (gameObject4 != null)
		{
			Health component6 = gameObject4.GetComponent<Health>();
			if (component6 != null && !component6.IsDefeated() && !gameObject4.HasTag(GameTags.Dead) && !gameObject4.HasTag(GameTags.Dying))
			{
				component6.Damage(20f);
				WoundMonitor.Instance smi = gameObject4.GetSMI<WoundMonitor.Instance>();
				if (smi != null && !component6.IsDefeated())
				{
					smi.PlayKnockedOverImpactAnimation();
				}
				gameObject4.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt(this.payload * 0.5f / 0.01f), "HEPImpact");
				this.Collide(HighEnergyParticle.CollisionType.Minion);
				return;
			}
		}
		if (Grid.IsSolidCell(cell))
		{
			GameObject gameObject5 = Grid.Objects[cell, 9];
			if (gameObject5 == null || !gameObject5.HasTag(GameTags.HEPPassThrough) || this.capturedBy == null || this.capturedBy.gameObject != gameObject5)
			{
				this.Collide(HighEnergyParticle.CollisionType.Solid);
			}
			return;
		}
	}

	// Token: 0x0600689B RID: 26779 RVA: 0x002D7D70 File Offset: 0x002D5F70
	public void MovingUpdate(float dt)
	{
		if (this.collision != HighEnergyParticle.CollisionType.None)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		Vector3 vector = position + EightDirectionUtil.GetNormal(this.direction) * this.speed * dt;
		int num2 = Grid.PosToCell(vector);
		SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance += this.speed * dt;
		this.loopingSounds.UpdateVelocity(this.flyingSound, vector - position);
		if (!Grid.IsValidCell(num2))
		{
			base.smi.sm.destroySimpleSignal.Trigger(base.smi);
			return;
		}
		if (num != num2)
		{
			this.payload -= 0.1f;
			byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id);
			int disease_delta = Mathf.FloorToInt(5f);
			if (!Grid.Element[num2].IsVacuum)
			{
				SimMessages.ModifyDiseaseOnCell(num2, index, disease_delta);
			}
		}
		if (this.payload <= 0f)
		{
			base.smi.sm.destroySimpleSignal.Trigger(base.smi);
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x0600689C RID: 26780 RVA: 0x000E490E File Offset: 0x000E2B0E
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
	}

	// Token: 0x0600689D RID: 26781 RVA: 0x000E4922 File Offset: 0x000E2B22
	private void StopLoopingSound()
	{
		this.loopingSounds.StopSound(this.flyingSound);
	}

	// Token: 0x04004ECF RID: 20175
	[Serialize]
	private EightDirection direction;

	// Token: 0x04004ED0 RID: 20176
	[Serialize]
	public float speed;

	// Token: 0x04004ED1 RID: 20177
	[Serialize]
	public float payload;

	// Token: 0x04004ED2 RID: 20178
	[MyCmpReq]
	private RadiationEmitter emitter;

	// Token: 0x04004ED3 RID: 20179
	[Serialize]
	public float perCellFalloff;

	// Token: 0x04004ED4 RID: 20180
	[Serialize]
	public HighEnergyParticle.CollisionType collision;

	// Token: 0x04004ED5 RID: 20181
	[Serialize]
	public HighEnergyParticlePort capturedBy;

	// Token: 0x04004ED6 RID: 20182
	public short emitRadius;

	// Token: 0x04004ED7 RID: 20183
	public float emitRate;

	// Token: 0x04004ED8 RID: 20184
	public float emitSpeed;

	// Token: 0x04004ED9 RID: 20185
	private LoopingSounds loopingSounds;

	// Token: 0x04004EDA RID: 20186
	public string flyingSound;

	// Token: 0x04004EDB RID: 20187
	public bool isCollideable;

	// Token: 0x020013E6 RID: 5094
	public enum CollisionType
	{
		// Token: 0x04004EDD RID: 20189
		None,
		// Token: 0x04004EDE RID: 20190
		Solid,
		// Token: 0x04004EDF RID: 20191
		Creature,
		// Token: 0x04004EE0 RID: 20192
		Minion,
		// Token: 0x04004EE1 RID: 20193
		Captured,
		// Token: 0x04004EE2 RID: 20194
		HighEnergyParticle,
		// Token: 0x04004EE3 RID: 20195
		CaptureAndRelease,
		// Token: 0x04004EE4 RID: 20196
		PassThrough
	}

	// Token: 0x020013E7 RID: 5095
	public class StatesInstance : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.GameInstance
	{
		// Token: 0x0600689F RID: 26783 RVA: 0x000E493D File Offset: 0x000E2B3D
		public StatesInstance(HighEnergyParticle smi) : base(smi)
		{
		}
	}

	// Token: 0x020013E8 RID: 5096
	public class States : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle>
	{
		// Token: 0x060068A0 RID: 26784 RVA: 0x002D7EBC File Offset: 0x002D60BC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.pre;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.ready.OnSignal(this.destroySimpleSignal, this.destroying.instant).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Creature).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Minion).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Solid).OnSignal(this.destroySignal, this.destroying.blackhole, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.HighEnergyParticle).OnSignal(this.destroySignal, this.destroying.captured, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Captured).OnSignal(this.destroySignal, this.catchAndRelease, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.CaptureAndRelease).Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(true);
				smi.master.isCollideable = true;
			}).Update(delegate(HighEnergyParticle.StatesInstance smi, float dt)
			{
				smi.master.MovingUpdate(dt);
				smi.master.CheckCollision();
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.pre.PlayAnim("travel_pre").OnAnimQueueComplete(this.ready.moving);
			this.ready.moving.PlayAnim("travel_loop", KAnim.PlayMode.Loop);
			this.catchAndRelease.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.collision = HighEnergyParticle.CollisionType.None;
			}).PlayAnim("explode", KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.pre);
			this.destroying.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.isCollideable = false;
				smi.master.StopLoopingSound();
			});
			this.destroying.instant.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				UnityEngine.Object.Destroy(smi.master.gameObject);
			});
			this.destroying.explode.PlayAnim("explode").Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				this.EmitRemainingPayload(smi);
			});
			this.destroying.blackhole.PlayAnim("collision").Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				this.EmitRemainingPayload(smi);
			});
			this.destroying.captured.PlayAnim("travel_pst").OnAnimQueueComplete(this.destroying.instant).Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
		}

		// Token: 0x060068A1 RID: 26785 RVA: 0x002D81F4 File Offset: 0x002D63F4
		private void EmitRemainingPayload(HighEnergyParticle.StatesInstance smi)
		{
			smi.master.GetComponent<KBatchedAnimController>().GetCurrentAnim();
			smi.master.emitter.emitRadiusX = 6;
			smi.master.emitter.emitRadiusY = 6;
			smi.master.emitter.emitRads = smi.master.payload * 0.5f * 600f / 9f;
			smi.master.emitter.Refresh();
			SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.gameObject), SimHashes.Fallout, CellEventLogger.Instance.ElementEmitted, smi.master.payload * 0.001f, 5000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt(smi.master.payload * 0.5f / 0.01f), true, -1);
			smi.Schedule(1f, delegate(object obj)
			{
				UnityEngine.Object.Destroy(smi.master.gameObject);
			}, null);
		}

		// Token: 0x04004EE5 RID: 20197
		public HighEnergyParticle.States.ReadyStates ready;

		// Token: 0x04004EE6 RID: 20198
		public HighEnergyParticle.States.DestructionStates destroying;

		// Token: 0x04004EE7 RID: 20199
		public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State catchAndRelease;

		// Token: 0x04004EE8 RID: 20200
		public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySignal;

		// Token: 0x04004EE9 RID: 20201
		public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySimpleSignal;

		// Token: 0x020013E9 RID: 5097
		public class ReadyStates : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
		{
			// Token: 0x04004EEA RID: 20202
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State pre;

			// Token: 0x04004EEB RID: 20203
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State moving;
		}

		// Token: 0x020013EA RID: 5098
		public class DestructionStates : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
		{
			// Token: 0x04004EEC RID: 20204
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State instant;

			// Token: 0x04004EED RID: 20205
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State explode;

			// Token: 0x04004EEE RID: 20206
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State captured;

			// Token: 0x04004EEF RID: 20207
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State blackhole;
		}
	}
}
