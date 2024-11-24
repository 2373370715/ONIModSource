using System;
using Database;
using UnityEngine;

// Token: 0x02001879 RID: 6265
public class SkyVisibilityMonitor : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>
{
	// Token: 0x060081A2 RID: 33186 RVA: 0x000F53BA File Offset: 0x000F35BA
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<SkyVisibilityMonitor.Instance, float>(SkyVisibilityMonitor.CheckSkyVisibility), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x060081A3 RID: 33187 RVA: 0x0033A1EC File Offset: 0x003383EC
	public static void CheckSkyVisibility(SkyVisibilityMonitor.Instance smi, float dt)
	{
		bool hasSkyVisibility = smi.HasSkyVisibility;
		ValueTuple<bool, float> visibilityOf = smi.def.skyVisibilityInfo.GetVisibilityOf(smi.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		smi.Internal_SetPercentClearSky(item2);
		KSelectable component = smi.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, smi);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, smi);
		if (hasSkyVisibility == item)
		{
			return;
		}
		smi.TriggerVisibilityChange();
	}

	// Token: 0x0200187A RID: 6266
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006268 RID: 25192
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x0200187B RID: 6267
	public new class Instance : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>.GameInstance, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060081A6 RID: 33190 RVA: 0x000F53E6 File Offset: 0x000F35E6
		public bool HasSkyVisibility
		{
			get
			{
				return this.PercentClearSky > 0f && !Mathf.Approximately(0f, this.PercentClearSky);
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060081A7 RID: 33191 RVA: 0x000F540A File Offset: 0x000F360A
		public float PercentClearSky
		{
			get
			{
				return this.percentClearSky01;
			}
		}

		// Token: 0x060081A8 RID: 33192 RVA: 0x000F5412 File Offset: 0x000F3612
		public void Internal_SetPercentClearSky(float percent01)
		{
			this.percentClearSky01 = percent01;
		}

		// Token: 0x060081A9 RID: 33193 RVA: 0x000F540A File Offset: 0x000F360A
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.percentClearSky01;
		}

		// Token: 0x060081AA RID: 33194 RVA: 0x000F541B File Offset: 0x000F361B
		public Instance(IStateMachineTarget master, SkyVisibilityMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060081AB RID: 33195 RVA: 0x000F5425 File Offset: 0x000F3625
		public override void StartSM()
		{
			base.StartSM();
			SkyVisibilityMonitor.CheckSkyVisibility(this, 0f);
			this.TriggerVisibilityChange();
		}

		// Token: 0x060081AC RID: 33196 RVA: 0x0033A278 File Offset: 0x00338478
		public void TriggerVisibilityChange()
		{
			if (this.visibilityStatusItem != null)
			{
				base.smi.GetComponent<KSelectable>().ToggleStatusItem(this.visibilityStatusItem, !this.HasSkyVisibility, this);
			}
			base.smi.GetComponent<Operational>().SetFlag(SkyVisibilityMonitor.Instance.skyVisibilityFlag, this.HasSkyVisibility);
			if (this.SkyVisibilityChanged != null)
			{
				this.SkyVisibilityChanged();
			}
		}

		// Token: 0x04006269 RID: 25193
		private float percentClearSky01;

		// Token: 0x0400626A RID: 25194
		public System.Action SkyVisibilityChanged;

		// Token: 0x0400626B RID: 25195
		private StatusItem visibilityStatusItem;

		// Token: 0x0400626C RID: 25196
		private static readonly Operational.Flag skyVisibilityFlag = new Operational.Flag("sky visibility", Operational.Flag.Type.Requirement);
	}
}
