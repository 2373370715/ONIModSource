using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001142 RID: 4418
public class CreatureLightToggleController : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>
{
	// Token: 0x06005A59 RID: 23129 RVA: 0x00294420 File Offset: 0x00292620
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.light_off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.light_off.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(false);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_on, new Func<CreatureLightToggleController.Instance, object, bool>(CreatureLightToggleController.ShouldProduceLight));
		this.turning_off.BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.dim, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_off, (CreatureLightToggleController.Instance smi) => smi.IsOff(), UpdateRate.SIM_200ms);
		this.light_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_off, (CreatureLightToggleController.Instance smi, object obj) => !CreatureLightToggleController.ShouldProduceLight(smi, obj));
		this.turning_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.brighten, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_on, (CreatureLightToggleController.Instance smi) => smi.IsOn(), UpdateRate.SIM_200ms);
	}

	// Token: 0x06005A5A RID: 23130 RVA: 0x000DAE03 File Offset: 0x000D9003
	public static bool ShouldProduceLight(CreatureLightToggleController.Instance smi, object obj)
	{
		return !smi.prefabID.HasTag(GameTags.Creatures.Overcrowded) && !smi.prefabID.HasTag(GameTags.Creatures.TrappedInCargoBay);
	}

	// Token: 0x04003FB9 RID: 16313
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_off;

	// Token: 0x04003FBA RID: 16314
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_off;

	// Token: 0x04003FBB RID: 16315
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_on;

	// Token: 0x04003FBC RID: 16316
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_on;

	// Token: 0x02001143 RID: 4419
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001144 RID: 4420
	public new class Instance : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.GameInstance
	{
		// Token: 0x06005A5D RID: 23133 RVA: 0x002945B0 File Offset: 0x002927B0
		public Instance(IStateMachineTarget master, CreatureLightToggleController.Def def) : base(master, def)
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			this.light = master.GetComponent<Light2D>();
			this.originalLux = this.light.Lux;
			this.originalRange = this.light.Range;
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x000DAE36 File Offset: 0x000D9036
		public void SwitchLight(bool on)
		{
			this.light.enabled = on;
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x00294604 File Offset: 0x00292804
		public static void ModifyBrightness(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, CreatureLightToggleController.Instance.ModifyLuxDelegate modify_lux, float time_delta)
		{
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
			for (int num = 0; num != instances.Count; num++)
			{
				UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry entry = instances[num];
				entry.lastUpdateTime = 0f;
				instances[num] = entry;
				CreatureLightToggleController.Instance data = entry.data;
				modify_lux(data, time_delta);
				data.light.Range = data.originalRange * (float)data.light.Lux / (float)data.originalLux;
				data.light.RefreshShapeAndPosition();
				if (data.light.RefreshShapeAndPosition() != Light2D.RefreshResult.None)
				{
					CreatureLightToggleController.Instance.modify_brightness_job.Add(new CreatureLightToggleController.Instance.ModifyBrightnessTask(data.light.emitter));
				}
			}
			GlobalJobManager.Run(CreatureLightToggleController.Instance.modify_brightness_job);
			for (int num2 = 0; num2 != CreatureLightToggleController.Instance.modify_brightness_job.Count; num2++)
			{
				CreatureLightToggleController.Instance.modify_brightness_job.GetWorkItem(num2).Finish();
			}
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x000DAE44 File Offset: 0x000D9044
		public bool IsOff()
		{
			return this.light.Lux == 0;
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x000DAE54 File Offset: 0x000D9054
		public bool IsOn()
		{
			return this.light.Lux >= this.originalLux;
		}

		// Token: 0x04003FBD RID: 16317
		private const float DIM_TIME = 25f;

		// Token: 0x04003FBE RID: 16318
		private const float GLOW_TIME = 15f;

		// Token: 0x04003FBF RID: 16319
		private int originalLux;

		// Token: 0x04003FC0 RID: 16320
		private float originalRange;

		// Token: 0x04003FC1 RID: 16321
		private Light2D light;

		// Token: 0x04003FC2 RID: 16322
		public KPrefabID prefabID;

		// Token: 0x04003FC3 RID: 16323
		private static WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object> modify_brightness_job = new WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object>();

		// Token: 0x04003FC4 RID: 16324
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate dim = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 25f;
			instance.light.Lux = Mathf.FloorToInt(Mathf.Max(0f, (float)instance.light.Lux - num * time_delta));
		};

		// Token: 0x04003FC5 RID: 16325
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate brighten = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 15f;
			instance.light.Lux = Mathf.CeilToInt(Mathf.Min((float)instance.originalLux, (float)instance.light.Lux + num * time_delta));
		};

		// Token: 0x02001145 RID: 4421
		private struct ModifyBrightnessTask : IWorkItem<object>
		{
			// Token: 0x06005A63 RID: 23139 RVA: 0x000DAEA2 File Offset: 0x000D90A2
			public ModifyBrightnessTask(LightGridManager.LightGridEmitter emitter)
			{
				this.emitter = emitter;
				emitter.RemoveFromGrid();
			}

			// Token: 0x06005A64 RID: 23140 RVA: 0x000DAEB1 File Offset: 0x000D90B1
			public void Run(object context)
			{
				this.emitter.UpdateLitCells();
			}

			// Token: 0x06005A65 RID: 23141 RVA: 0x000DAEBE File Offset: 0x000D90BE
			public void Finish()
			{
				this.emitter.AddToGrid(false);
			}

			// Token: 0x04003FC6 RID: 16326
			private LightGridManager.LightGridEmitter emitter;
		}

		// Token: 0x02001146 RID: 4422
		// (Invoke) Token: 0x06005A67 RID: 23143
		public delegate void ModifyLuxDelegate(CreatureLightToggleController.Instance instance, float time_delta);
	}
}
