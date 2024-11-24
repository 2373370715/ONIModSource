using System;
using UnityEngine;

public class Chlorinator : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>
{
	public class Def : BaseDef
	{
		public MathUtil.MinMax popWaitRange = new MathUtil.MinMax(0.2f, 0.8f);

		public Tag primaryOreTag;

		public float primaryOreMassPerOre;

		public MathUtil.MinMaxInt primaryOreCount = new MathUtil.MinMaxInt(1, 1);

		public Tag secondaryOreTag;

		public float secondaryOreMassPerOre;

		public MathUtil.MinMaxInt secondaryOreCount = new MathUtil.MinMaxInt(1, 1);

		public Vector3 offset = Vector3.zero;

		public MathUtil.MinMax initialVelocity = new MathUtil.MinMax(1f, 3f);

		public MathUtil.MinMax initialDirectionHalfAngleDegreesRange = new MathUtil.MinMax(160f, 20f);
	}

	public class ReadyStates : State
	{
		public State idle;

		public State wait;

		public State popPre;

		public State pop;

		public State popPst;
	}

	public class StatesInstance : GameInstance
	{
		public Storage storage;

		public MeterController hopperMeter;

		public StatesInstance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			storage = GetComponent<ComplexFabricator>().outStorage;
			KAnimControllerBase component = master.GetComponent<KAnimControllerBase>();
			hopperMeter = new MeterController(component, "meter_target", "meter_hopper_pre", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, "meter_target");
			base.sm.hopper.Set(hopperMeter.gameObject, this);
		}

		public bool CanEmit()
		{
			return !storage.IsEmpty();
		}

		public void TryEmit()
		{
			TryEmit(base.smi.def.primaryOreCount.Get(), base.def.primaryOreTag, base.def.primaryOreMassPerOre);
			TryEmit(base.smi.def.secondaryOreCount.Get(), base.def.secondaryOreTag, base.def.secondaryOreMassPerOre);
		}

		private void TryEmit(int oreSpawnCount, Tag emitTag, float amount)
		{
			GameObject gameObject = storage.FindFirst(emitTag);
			if (gameObject == null)
			{
				return;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			Substance substance = component.Element.substance;
			storage.ConsumeAndGetDisease(emitTag, amount, out var amount_consumed, out var disease_info, out var aggregate_temperature);
			if (amount_consumed <= 0f)
			{
				return;
			}
			float mass = amount_consumed * component.MassPerUnit / (float)oreSpawnCount;
			Vector3 position = base.smi.gameObject.transform.position;
			position += base.def.offset;
			bool flag = UnityEngine.Random.value >= 0.5f;
			for (int i = 0; i < oreSpawnCount; i++)
			{
				float f = base.def.initialDirectionHalfAngleDegreesRange.Get() * (float)Math.PI / 180f;
				Vector2 vector = new Vector2(0f - Mathf.Cos(f), Mathf.Sin(f));
				if (flag)
				{
					vector.x = 0f - vector.x;
				}
				flag = !flag;
				vector = vector.normalized;
				Vector3 vector2 = vector * base.def.initialVelocity.Get();
				Vector3 vector3 = position;
				vector3 += (Vector3)(vector * 0.1f);
				GameObject go = substance.SpawnResource(vector3, mass, aggregate_temperature, disease_info.idx, disease_info.count / oreSpawnCount);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Chlorinator_popping"), CameraController.Instance.GetVerticallyScaledPosition(vector3));
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, vector2);
			}
		}
	}

	private State inoperational;

	private ReadyStates ready;

	public TargetParameter hopper;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = inoperational;
		inoperational.TagTransition(GameTags.Operational, ready);
		ready.TagTransition(GameTags.Operational, inoperational, on_remove: true).DefaultState(ready.idle);
		ready.idle.EventTransition(GameHashes.OnStorageChange, ready.wait, (StatesInstance smi) => smi.CanEmit()).EnterTransition(ready.wait, (StatesInstance smi) => smi.CanEmit()).Target(hopper)
			.PlayAnim("hopper_idle_loop");
		ready.wait.ScheduleGoTo(GetPoppingDelay, ready.popPre).EnterTransition(ready.idle, (StatesInstance smi) => !smi.CanEmit()).Target(hopper)
			.PlayAnim("hopper_idle_loop");
		ready.popPre.Target(hopper).PlayAnim("meter_hopper_pre").OnAnimQueueComplete(ready.pop);
		ready.pop.Enter(delegate(StatesInstance smi)
		{
			smi.TryEmit();
		}).Target(hopper).PlayAnim("meter_hopper_loop")
			.OnAnimQueueComplete(ready.popPst);
		ready.popPst.Target(hopper).PlayAnim("meter_hopper_pst").OnAnimQueueComplete(ready.wait);
	}

	public static float GetPoppingDelay(StatesInstance smi)
	{
		return smi.def.popWaitRange.Get();
	}
}
