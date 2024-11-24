using System;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>
{
	public class Def : BaseDef
	{
		public static readonly CellOffset LaunchOffset = new CellOffset(0, 4);

		public float launchSpeed = 30f;

		public float rotationSpeed = 100f;

		public static readonly Vector2I launchRange = new Vector2I(16, 32);

		public float scanningAngle = 50f;

		public float maxAngle = 80f;
	}

	public new class Instance : GameInstance
	{
		[MyCmpReq]
		public Operational Operational;

		[MyCmpReq]
		public Storage MissileStorage;

		[MyCmpReq]
		public KSelectable Selectable;

		[MyCmpReq]
		public FlatTagFilterable TargetFilter;

		private Vector3 launchPosition;

		private Vector2I launchXY;

		private float launchAnimTime;

		public KBatchedAnimController cannonAnimController;

		public GameObject cannonGameObject;

		public float cannonRotation;

		public float simpleAngle;

		private Tag missileElement;

		private MeterController meter;

		private WorldContainer worldContainer;

		public WorldContainer myWorld
		{
			get
			{
				if (worldContainer == null)
				{
					worldContainer = this.GetMyWorld();
				}
				return worldContainer;
			}
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			string name = component.name + ".cannon";
			base.smi.cannonGameObject = new GameObject(name);
			base.smi.cannonGameObject.SetActive(value: false);
			base.smi.cannonGameObject.transform.parent = component.transform;
			base.smi.cannonGameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
			base.smi.cannonAnimController = base.smi.cannonGameObject.AddComponent<KBatchedAnimController>();
			base.smi.cannonAnimController.AnimFiles = new KAnimFile[1] { component.AnimFiles[0] };
			base.smi.cannonAnimController.initialAnim = "Cannon_off";
			base.smi.cannonAnimController.isMovable = true;
			base.smi.cannonAnimController.SetSceneLayer(Grid.SceneLayer.Building);
			component.SetSymbolVisiblity("cannon_target", is_visible: false);
			bool symbolVisible;
			Vector3 position = component.GetSymbolTransform(new HashedString("cannon_target"), out symbolVisible).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Building);
			base.smi.cannonGameObject.transform.SetPosition(position);
			launchPosition = position;
			Grid.PosToXY(launchPosition, out launchXY);
			base.smi.cannonGameObject.SetActive(value: true);
			base.smi.sm.cannonTarget.Set(base.smi.cannonGameObject, base.smi);
			KAnim.Anim anim = component.AnimFiles[0].GetData().GetAnim("Cannon_shooting_pre");
			if (anim != null)
			{
				launchAnimTime = anim.totalTime / 2f;
			}
			else
			{
				Debug.LogWarning("MissileLauncher anim data is missing");
				launchAnimTime = 1f;
			}
			meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer);
			Subscribe(-1201923725, OnHighlight);
			MissileStorage.Subscribe(-1697596308, OnStorage);
			FlatTagFilterable component2 = base.smi.master.GetComponent<FlatTagFilterable>();
			foreach (GameObject item in Assets.GetPrefabsWithTag(GameTags.Comet))
			{
				if (!item.HasTag(GameTags.DeprecatedContent))
				{
					if (!component2.tagOptions.Contains(item.PrefabID()))
					{
						component2.tagOptions.Add(item.PrefabID());
						component2.selectedTags.Add(item.PrefabID());
					}
					component2.selectedTags.Remove(GassyMooCometConfig.ID);
				}
			}
		}

		public override void StartSM()
		{
			base.StartSM();
			OnStorage(null);
		}

		protected override void OnCleanUp()
		{
			Unsubscribe(-1201923725, OnHighlight);
			base.OnCleanUp();
		}

		private void OnHighlight(object data)
		{
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			base.smi.cannonAnimController.HighlightColour = component.HighlightColour;
		}

		private void OnStorage(object data)
		{
			meter.SetPositionPercent(Mathf.Clamp01(MissileStorage.MassStored() / MissileStorage.capacityKg));
		}

		public void Searching(float dt)
		{
			FindMeteor();
			RotateCannon(dt, base.def.rotationSpeed / 2f);
			if (base.smi.sm.rotationComplete.Get(base.smi))
			{
				cannonRotation *= -1f;
				base.smi.sm.rotationComplete.Set(value: false, base.smi);
			}
		}

		public void FindMeteor()
		{
			GameObject gameObject = ChooseClosestInterceptionPoint(myWorld.id);
			if (gameObject != null)
			{
				base.smi.sm.meteorTarget.Set(gameObject, base.smi);
				gameObject.GetComponent<Comet>().Targeted = true;
				base.smi.cannonRotation = CalculateLaunchAngle(gameObject.transform.position);
			}
		}

		private float CalculateLaunchAngle(Vector3 targetPosition)
		{
			Vector3 v = Vector3.Normalize(targetPosition - launchPosition);
			return MathUtil.AngleSigned(Vector3.up, v, Vector3.forward);
		}

		public void LaunchMissile()
		{
			GameObject gameObject = MissileStorage.FindFirst("MissileBasic");
			if (gameObject != null)
			{
				Pickupable pickupable = gameObject.GetComponent<Pickupable>();
				if (pickupable.TotalAmount <= 1f)
				{
					MissileStorage.Drop(pickupable.gameObject);
				}
				else
				{
					pickupable = EntitySplitter.Split(pickupable, 1f);
				}
				SetMissileElement(gameObject);
				GameObject gameObject2 = base.smi.sm.meteorTarget.Get(base.smi);
				if (!gameObject2.IsNullOrDestroyed())
				{
					pickupable.GetSMI<MissileProjectile.StatesInstance>().PrepareLaunch(gameObject2.GetComponent<Comet>(), base.def.launchSpeed, launchPosition, base.smi.cannonRotation);
				}
			}
		}

		private void SetMissileElement(GameObject missile)
		{
			missileElement = missile.GetComponent<PrimaryElement>().Element.tag;
			if (Assets.GetPrefab(missileElement) == null)
			{
				Debug.LogWarning($"Missing element {missileElement} for missile launcher. Defaulting to IronOre");
				missileElement = GameTags.IronOre;
			}
		}

		public GameObject ChooseClosestInterceptionPoint(int world_id)
		{
			GameObject result = null;
			List<Comet> items = Components.Meteors.GetItems(world_id);
			float num = Def.launchRange.y;
			foreach (Comet item in items)
			{
				if (!item.IsNullOrDestroyed() && !item.Targeted && TargetFilter.selectedTags.Contains(item.typeID))
				{
					Vector3 targetPosition = item.TargetPosition;
					float timeToCollision;
					Vector3 vector = CalculateCollisionPoint(targetPosition, item.Velocity, out timeToCollision);
					Grid.PosToCell(vector);
					float num2 = Vector3.Distance(vector, launchPosition);
					if (num2 < num && timeToCollision > launchAnimTime && IsMeteorInRange(vector) && IsPathClear(launchPosition, targetPosition))
					{
						result = item.gameObject;
						num = num2;
					}
				}
			}
			return result;
		}

		private bool IsMeteorInRange(Vector3 interception_point)
		{
			Grid.PosToXY(interception_point, out var xy);
			if (Math.Abs(xy.X - launchXY.X) <= Def.launchRange.X && xy.Y - launchXY.Y > 0)
			{
				return xy.Y - launchXY.Y <= Def.launchRange.Y;
			}
			return false;
		}

		public bool IsPathClear(Vector3 startPoint, Vector3 endPoint)
		{
			Vector2I vector2I = Grid.PosToXY(startPoint);
			Vector2I vector2I2 = Grid.PosToXY(endPoint);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, IsCellBlockedFromSky, blocking_tile_visible: false, allow_invalid_cells: true);
		}

		public bool IsCellBlockedFromSky(int cell)
		{
			if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] == myWorld.id)
			{
				return Grid.Solid[cell];
			}
			Grid.CellToXY(cell, out var _, out var y);
			return y <= launchXY.Y;
		}

		public Vector3 CalculateCollisionPoint(Vector3 targetPosition, Vector3 targetVelocity, out float timeToCollision)
		{
			Vector3 vector = targetVelocity - base.smi.def.launchSpeed * (targetPosition - launchPosition).normalized;
			timeToCollision = (targetPosition - launchPosition).magnitude / vector.magnitude;
			return targetPosition + targetVelocity * timeToCollision;
		}

		public void HasLineOfSight()
		{
			bool flag = false;
			bool flag2 = true;
			Extents extents = GetComponent<Building>().GetExtents();
			int val = launchXY.x - Def.launchRange.X;
			int val2 = launchXY.x + Def.launchRange.X;
			int y = extents.y + extents.height;
			int num = Grid.XYToCell(Math.Max((int)myWorld.minimumBounds.x, val), y);
			int num2 = Grid.XYToCell(Math.Min((int)myWorld.maximumBounds.x, val2), y);
			for (int i = num; i <= num2; i++)
			{
				flag = flag || Grid.ExposedToSunlight[i] <= 0;
				flag2 = flag2 && Grid.ExposedToSunlight[i] <= 0;
			}
			Selectable.ToggleStatusItem(PartiallyBlockedStatus, flag && !flag2);
			Selectable.ToggleStatusItem(NoSurfaceSight, flag2);
			base.smi.sm.fullyBlocked.Set(flag2, base.smi);
		}

		public bool MeteorDetected()
		{
			return Components.Meteors.GetItems(myWorld.id).Count > 0;
		}

		public void SetOreChunk()
		{
			if (!missileElement.IsValid)
			{
				Debug.LogWarning($"Missing element {missileElement} for missile launcher. Defaulting to IronOre");
				missileElement = GameTags.IronOre;
			}
			KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(missileElement).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0u);
			base.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("Shell", symbolByIndex);
		}

		public void SpawnOre()
		{
			bool symbolVisible;
			Vector3 position = GetComponent<KBatchedAnimController>().GetSymbolTransform("Shell", out symbolVisible).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Assets.GetPrefab(missileElement).GetComponent<PrimaryElement>().Element.substance.SpawnResource(position, SHELL_MASS, SHELL_TEMPERATURE, byte.MaxValue, 0);
		}

		public void RotateCannon(float dt, float rotation_speed)
		{
			float num = cannonRotation - simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = rotation_speed * dt;
			if (num > 0f && num2 < num)
			{
				simpleAngle += num2;
				cannonAnimController.Rotation = simpleAngle;
			}
			else if (num < 0f && 0f - num2 > num)
			{
				simpleAngle -= num2;
				cannonAnimController.Rotation = simpleAngle;
			}
			else
			{
				simpleAngle = cannonRotation;
				cannonAnimController.Rotation = simpleAngle;
				base.smi.sm.rotationComplete.Set(value: true, base.smi);
			}
		}

		public void RotateToMeteor(float dt)
		{
			GameObject gameObject = base.sm.meteorTarget.Get(this);
			if (!gameObject.IsNullOrDestroyed())
			{
				float num = CalculateLaunchAngle(gameObject.transform.position) - simpleAngle;
				if (num > 180f)
				{
					num -= 360f;
				}
				else if (num < -180f)
				{
					num += 360f;
				}
				float num2 = base.def.rotationSpeed * dt;
				if (num > 0f && num2 < num)
				{
					simpleAngle += num2;
					cannonAnimController.Rotation = simpleAngle;
				}
				else if (num < 0f && 0f - num2 > num)
				{
					simpleAngle -= num2;
					cannonAnimController.Rotation = simpleAngle;
				}
				else
				{
					base.smi.sm.rotationComplete.Set(value: true, base.smi);
				}
			}
		}
	}

	public class OnState : State
	{
		public State searching;

		public State opening;

		public State shutdown;

		public State idle;
	}

	public class LaunchState : State
	{
		public State targeting;

		public State shoot;

		public State pst;
	}

	public class CooldownState : State
	{
		public State cooling;

		public State exit;

		public State exitNoAmmo;
	}

	private static StatusItem NoSurfaceSight = new StatusItem("MissileLauncher_NoSurfaceSight", "BUILDING", "status_item_no_sky", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);

	private static StatusItem PartiallyBlockedStatus = new StatusItem("MissileLauncher_PartiallyBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);

	public float shutdownDuration = 50f;

	public float shootDelayDuration = 0.25f;

	public static float SHELL_MASS = MissileBasicConfig.recipe.ingredients[0].amount / 5f / 2f;

	public static float SHELL_TEMPERATURE = 353.15f;

	public BoolParameter rotationComplete;

	public ObjectParameter<GameObject> meteorTarget = new ObjectParameter<GameObject>();

	public TargetParameter cannonTarget;

	public BoolParameter fullyBlocked;

	public State Off;

	public OnState On;

	public LaunchState Launch;

	public CooldownState Cooldown;

	public State Nosurfacesight;

	public State NoAmmo;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = Off;
		root.Update(delegate(Instance smi, float dt)
		{
			smi.HasLineOfSight();
		});
		Off.PlayAnim("inoperational").EventTransition(GameHashes.OperationalChanged, On, (Instance smi) => smi.Operational.IsOperational).Enter(delegate(Instance smi)
		{
			smi.Operational.SetActive(value: false);
		});
		On.DefaultState(On.opening).EventTransition(GameHashes.OperationalChanged, On.shutdown, (Instance smi) => !smi.Operational.IsOperational).ParamTransition(fullyBlocked, Nosurfacesight, GameStateMachine<MissileLauncher, Instance, IStateMachineTarget, Def>.IsTrue)
			.ScheduleGoTo(shutdownDuration, On.idle)
			.Enter(delegate(Instance smi)
			{
				smi.Operational.SetActive(smi.Operational.IsOperational);
			});
		On.opening.PlayAnim("working_pre").OnAnimQueueComplete(On.searching).Target(cannonTarget)
			.PlayAnim("Cannon_working_pre");
		On.searching.PlayAnim("on", KAnim.PlayMode.Loop).Enter(delegate(Instance smi)
		{
			smi.sm.rotationComplete.Set(value: false, smi);
			smi.sm.meteorTarget.Set(null, smi);
			smi.cannonRotation = smi.def.scanningAngle;
		}).Update("FindMeteor", delegate(Instance smi, float dt)
		{
			smi.Searching(dt);
		}, UpdateRate.SIM_EVERY_TICK)
			.EventTransition(GameHashes.OnStorageChange, NoAmmo, (Instance smi) => smi.MissileStorage.Count <= 0)
			.ParamTransition(meteorTarget, Launch.targeting, (Instance smi, GameObject meteor) => meteor != null)
			.Exit(delegate(Instance smi)
			{
				smi.sm.rotationComplete.Set(value: false, smi);
			});
		On.idle.Target(masterTarget).PlayAnim("idle", KAnim.PlayMode.Loop).UpdateTransition(On, (Instance smi, float dt) => smi.Operational.IsOperational && smi.MeteorDetected())
			.Target(cannonTarget)
			.PlayAnim("Cannon_working_pst");
		On.shutdown.Target(masterTarget).PlayAnim("working_pst").OnAnimQueueComplete(Off)
			.Target(cannonTarget)
			.PlayAnim("Cannon_working_pst");
		Launch.PlayAnim("target_detected", KAnim.PlayMode.Loop).Update("Rotate", delegate(Instance smi, float dt)
		{
			smi.RotateToMeteor(dt);
		}, UpdateRate.SIM_EVERY_TICK);
		Launch.targeting.Update("Targeting", delegate(Instance smi, float dt)
		{
			if (smi.sm.meteorTarget.Get(smi).IsNullOrDestroyed())
			{
				smi.GoTo(On.searching);
			}
			else if (smi.cannonAnimController.Rotation < smi.def.maxAngle * -1f || smi.cannonAnimController.Rotation > smi.def.maxAngle)
			{
				smi.sm.meteorTarget.Get(smi).GetComponent<Comet>().Targeted = false;
				smi.sm.meteorTarget.Set(null, smi);
				smi.GoTo(On.searching);
			}
		}, UpdateRate.SIM_EVERY_TICK).ParamTransition(rotationComplete, Launch.shoot, GameStateMachine<MissileLauncher, Instance, IStateMachineTarget, Def>.IsTrue);
		Launch.shoot.ScheduleGoTo(shootDelayDuration, Launch.pst).Exit("LaunchMissile", delegate(Instance smi)
		{
			smi.LaunchMissile();
			cannonTarget.Get(smi).GetComponent<KBatchedAnimController>().Play("Cannon_shooting_pre");
		});
		Launch.pst.Target(masterTarget).Enter(delegate(Instance smi)
		{
			smi.SetOreChunk();
			KAnimControllerBase component2 = smi.GetComponent<KAnimControllerBase>();
			if (smi.GetComponent<Storage>().Count <= 0)
			{
				component2.Play("base_shooting_pst_last");
			}
			else
			{
				component2.Play("base_shooting_pst");
			}
		}).Target(cannonTarget)
			.PlayAnim("Cannon_shooting_pst")
			.OnAnimQueueComplete(Cooldown);
		Cooldown.Update("Rotate", delegate(Instance smi, float dt)
		{
			smi.RotateToMeteor(dt);
		}, UpdateRate.SIM_EVERY_TICK).Exit(delegate(Instance smi)
		{
			smi.SpawnOre();
		}).Enter(delegate(Instance smi)
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			if (smi.GetComponent<Storage>().Count <= 0)
			{
				component.Play("base_ejecting_last");
			}
			else
			{
				component.Play("base_ejecting");
			}
			smi.sm.rotationComplete.Set(value: false, smi);
			smi.sm.meteorTarget.Set(null, smi);
		})
			.OnAnimQueueComplete(On.searching);
		Nosurfacesight.Target(masterTarget).PlayAnim("working_pst").QueueAnim("error")
			.ParamTransition(fullyBlocked, On, GameStateMachine<MissileLauncher, Instance, IStateMachineTarget, Def>.IsFalse)
			.Target(cannonTarget)
			.PlayAnim("Cannon_working_pst")
			.Enter(delegate(Instance smi)
			{
				smi.Operational.SetActive(value: false);
			});
		NoAmmo.PlayAnim("off_open").EventTransition(GameHashes.OnStorageChange, On, (Instance smi) => smi.MissileStorage.Count > 0).Enter(delegate(Instance smi)
		{
			smi.Operational.SetActive(value: false);
		})
			.Exit(delegate(Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play("off_closing");
			})
			.Target(cannonTarget)
			.PlayAnim("Cannon_working_pst");
	}
}
