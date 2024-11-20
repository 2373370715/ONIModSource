using System;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class MissileProjectile : GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ParamTransition<Comet>(this.meteorTarget, this.launch, (MissileProjectile.StatesInstance smi, Comet comet) => comet != null);
		this.launch.Update("Launch", delegate(MissileProjectile.StatesInstance smi, float dt)
		{
			smi.UpdateLaunch(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<bool>(this.triggerexplode, this.explode, GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.IsTrue).Enter(delegate(MissileProjectile.StatesInstance smi)
		{
			Vector3 position = smi.master.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
			smi.smokeTrailFX = Util.KInstantiate(EffectPrefabs.Instance.MissileSmokeTrailFX, position);
			smi.smokeTrailFX.transform.SetParent(smi.master.transform);
			smi.smokeTrailFX.SetActive(true);
			smi.StartTakeoff();
			KFMOD.PlayOneShot(GlobalAssets.GetSound("MissileLauncher_Missile_ignite", false), CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		});
		this.explode.Enter(delegate(MissileProjectile.StatesInstance smi)
		{
			smi.TriggerExplosion();
			ParticleSystem[] componentsInChildren = smi.smokeTrailFX.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].emission.enabled = false;
			}
		});
	}

	public GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.State launch;

	public GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.State explode;

	public StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.BoolParameter triggerexplode = new StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.BoolParameter(false);

	public StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.ObjectParameter<Comet> meteorTarget = new StateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.ObjectParameter<Comet>();

	public class Def : StateMachine.BaseDef
	{
		public float MeteorDebrisMassModifier = 0.25f;

		public float ExplosionRange = 2f;

		public float debrisSpeed = 6f;

		public float debrisMaxAngle = 40f;

		public string explosionEffectAnim = "missile_explosion_kanim";
	}

	public class StatesInstance : GameStateMachine<MissileProjectile, MissileProjectile.StatesInstance, IStateMachineTarget, MissileProjectile.Def>.GameInstance
	{
				private Vector3 Position
		{
			get
			{
				return base.transform.position + this.animController.Offset;
			}
		}

		public StatesInstance(IStateMachineTarget master, MissileProjectile.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
		}

		public void StartTakeoff()
		{
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		public void UpdateLaunch(float dt)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			Comet comet = base.sm.meteorTarget.Get(base.smi);
			if (!comet.IsNullOrDestroyed())
			{
				Vector3 targetPosition = comet.TargetPosition;
				base.sm.triggerexplode.Set(this.InExplosionRange(targetPosition, this.Position), base.smi, false);
				Vector3 v = Vector3.Normalize(targetPosition - this.Position);
				Vector3 normalized = (targetPosition - this.Position).normalized;
				float rotation = MathUtil.AngleSigned(Vector3.up, v, Vector3.forward);
				this.animController.Rotation = rotation;
				if (Grid.IsValidCellInWorld(Grid.PosToCell(this.Position), myWorldId))
				{
					base.transform.SetPosition(base.transform.position + normalized * (this.launchSpeed * dt));
				}
				else
				{
					this.animController.Offset += normalized * (this.launchSpeed * dt);
				}
				ParticleSystem[] componentsInChildren = base.smi.smokeTrailFX.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.transform.SetPositionAndRotation(this.Position, Quaternion.identity);
				}
				return;
			}
			if (!base.sm.triggerexplode.Get(base.smi))
			{
				if (!base.smi.smokeTrailFX.IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(base.smi.smokeTrailFX);
				}
				if (!GameComps.Fallers.Has(base.gameObject))
				{
					GameComps.Fallers.Add(base.gameObject, Vector2.down);
				}
				base.gameObject.GetComponent<KSelectable>().enabled = true;
				base.smi.GoTo("root");
			}
		}

		public void PrepareLaunch(Comet meteor_target, float speed, Vector3 launchPos, float launchAngle)
		{
			base.gameObject.transform.SetParent(null);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			launchPos.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
			base.gameObject.transform.SetLocalPosition(launchPos);
			this.animController.Rotation = launchAngle;
			this.animController.Offset = Vector3.back;
			this.animController.SetVisiblity(true);
			base.sm.triggerexplode.Set(false, base.smi, false);
			base.sm.meteorTarget.Set(meteor_target, base.smi, false);
			this.launchSpeed = speed;
		}

		public void TriggerExplosion()
		{
			if (!base.smi.sm.meteorTarget.IsNullOrDestroyed())
			{
				this.SpawnMeteorResources(base.smi.sm.meteorTarget.Get(base.smi));
				Util.KDestroyGameObject(base.smi.sm.meteorTarget.Get(base.smi));
			}
			this.Explode();
		}

		private void SpawnMeteorResources(Comet meteor)
		{
			PrimaryElement meteorPE = meteor.GetComponent<PrimaryElement>();
			Element element = meteorPE.Element;
			int num = meteor.GetMyWorldId();
			if (num == 255 || num == -1)
			{
				WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(meteor.transform.GetPosition() - Vector3.down * Grid.CellSizeInMeters);
				num = ((worldFromPosition == null) ? num : worldFromPosition.id);
			}
			bool flag = Grid.IsValidCellInWorld(Grid.PosToCell(meteor.TargetPosition), num);
			float num2 = meteor.ExplosionMass * base.def.MeteorDebrisMassModifier;
			float num3 = meteor.AddTileMass * base.def.MeteorDebrisMassModifier;
			int num_nonTiles_ores = meteor.GetRandomNumOres();
			float arg = (num_nonTiles_ores > 0) ? (num2 / (float)num_nonTiles_ores) : 1f;
			float temperature = meteor.GetRandomTemperatureForOres();
			int num_tile_ores = meteor.addTiles;
			float arg2 = (num_tile_ores > 0) ? (num3 / (float)num_tile_ores) : 1f;
			Vector3 normalized = (meteor.TargetPosition - this.Position).normalized;
			Vector2 vector = new Vector2(normalized.x, normalized.y);
			new Vector2(vector.y, -vector.x);
			Func<int, int, float, Vector3> func = delegate(int objectIndex, int objectCount, float maxAngleAllowed)
			{
				int num5 = (objectCount % 2 == 0) ? objectCount : (objectCount - 1);
				float num6 = maxAngleAllowed * 2f / (float)num5;
				bool flag2 = objectIndex % 2 == 0;
				float num7 = num6 * (float)Mathf.CeilToInt((float)objectIndex / 2f) * 0.017453292f * (float)(flag2 ? 1 : -1);
				Vector3 vector4 = new Vector3(Mathf.Cos(4.712389f + num7), Mathf.Sin(4.712389f + num7), 0f);
				return vector4.normalized * this.def.debrisSpeed;
			};
			Action<Substance, float, Vector3> action = delegate(Substance substance, float mass, Vector3 velocity)
			{
				Vector3 vector4 = velocity.normalized * 0.75f;
				vector4 += new Vector3(0f, 0.55f, 0f);
				vector4 += this.Position;
				GameObject go = substance.SpawnResource(vector4, mass, temperature, meteorPE.DiseaseIdx, meteorPE.DiseaseCount / (num_nonTiles_ores + num_tile_ores), false, false, false);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, velocity);
			};
			Action<string, Vector3> action2 = delegate(string prefabName, Vector3 velocity)
			{
				Vector3 vector4 = velocity.normalized * 0.75f;
				vector4 += new Vector3(0f, 0.55f, 0f);
				vector4 += this.Position;
				GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(vector4), 0, 0, prefabName, Grid.SceneLayer.Ore);
				gameObject.SetActive(true);
				vector4.z = gameObject.transform.position.z;
				gameObject.transform.position = vector4;
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, velocity);
			};
			Substance substance2 = element.substance;
			if (flag)
			{
				int arg3 = num_nonTiles_ores + num_tile_ores + ((meteor.lootOnDestroyedByMissile == null) ? 0 : meteor.lootOnDestroyedByMissile.Length);
				for (int i = 0; i < num_nonTiles_ores; i++)
				{
					Vector3 arg4 = func(i, arg3, base.def.debrisMaxAngle);
					action(substance2, arg, arg4);
				}
				for (int j = 0; j < num_tile_ores; j++)
				{
					Vector3 arg5 = func(num_nonTiles_ores + j, arg3, base.def.debrisMaxAngle);
					action(substance2, arg2, arg5);
				}
				if (meteor.lootOnDestroyedByMissile != null)
				{
					for (int k = 0; k < meteor.lootOnDestroyedByMissile.Length; k++)
					{
						Vector3 arg6 = func(num_nonTiles_ores + num_tile_ores + k, arg3, base.def.debrisMaxAngle);
						string arg7 = meteor.lootOnDestroyedByMissile[k];
						action2(arg7, arg6);
					}
					return;
				}
			}
			else if (num != -1 && num != 255)
			{
				int num4 = Grid.PosToCell(meteor.TargetPosition);
				Vector3 vector2 = meteor.TargetPosition;
				Vector2 vector3 = meteor.GetMyWorld().WorldOffset;
				while (!Grid.IsValidCellInWorld(num4, num) && vector2.y > vector3.y)
				{
					num4 = Grid.CellBelow(num4);
					vector2 = Grid.CellToPos(num4);
				}
				if (vector2.y > vector3.y)
				{
					substance2.SpawnResource(vector2, num2 + num3, temperature, meteorPE.DiseaseIdx, meteorPE.DiseaseCount, false, false, false);
					if (meteor.lootOnDestroyedByMissile != null)
					{
						for (int l = 0; l < meteor.lootOnDestroyedByMissile.Length; l++)
						{
							string name = meteor.lootOnDestroyedByMissile[l];
							Scenario.SpawnPrefab(num4, 0, 0, name, Grid.SceneLayer.Ore).SetActive(true);
						}
					}
				}
			}
		}

		private void Explode()
		{
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
			Vector3 position = base.gameObject.transform.position;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
			this.SpawnExplosionFX(base.def.explosionEffectAnim, position, this.animController.Offset);
			this.animController.SetSymbolVisiblity("missile_body", false);
			this.animController.SetSymbolVisiblity("missile_head", false);
		}

		private bool InExplosionRange(Vector3 target_pos, Vector3 current_pos)
		{
			return Vector2.Distance(target_pos, current_pos) <= base.def.ExplosionRange;
		}

		private void SpawnExplosionFX(string anim, Vector3 pos, Vector3 offset)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(anim, pos, base.gameObject.transform, false, Grid.SceneLayer.FXFront2, false);
			kbatchedAnimController.Offset = offset;
			kbatchedAnimController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
			kbatchedAnimController.onAnimComplete += delegate(HashedString obj)
			{
				Util.KDestroyGameObject(base.gameObject);
			};
		}

		public KBatchedAnimController animController;

		private float launchSpeed;

		public GameObject smokeTrailFX;
	}
}
