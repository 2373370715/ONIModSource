using System;
using Database;
using UnityEngine;

public class SkyVisibilityMonitor : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>
{
	public class Def : BaseDef
	{
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	public new class Instance : GameInstance, BuildingStatusItems.ISkyVisInfo
	{
		private float percentClearSky01;

		public System.Action SkyVisibilityChanged;

		private StatusItem visibilityStatusItem;

		private static readonly Operational.Flag skyVisibilityFlag = new Operational.Flag("sky visibility", Operational.Flag.Type.Requirement);

		public bool HasSkyVisibility
		{
			get
			{
				if (PercentClearSky > 0f)
				{
					return !Mathf.Approximately(0f, PercentClearSky);
				}
				return false;
			}
		}

		public float PercentClearSky => percentClearSky01;

		public void Internal_SetPercentClearSky(float percent01)
		{
			percentClearSky01 = percent01;
		}

		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return percentClearSky01;
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			CheckSkyVisibility(this, 0f);
			TriggerVisibilityChange();
		}

		public void TriggerVisibilityChange()
		{
			if (visibilityStatusItem != null)
			{
				base.smi.GetComponent<KSelectable>().ToggleStatusItem(visibilityStatusItem, !HasSkyVisibility, this);
			}
			base.smi.GetComponent<Operational>().SetFlag(skyVisibilityFlag, HasSkyVisibility);
			if (SkyVisibilityChanged != null)
			{
				SkyVisibilityChanged();
			}
		}
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = root;
		root.Update(CheckSkyVisibility, UpdateRate.SIM_1000ms);
	}

	public static void CheckSkyVisibility(Instance smi, float dt)
	{
		bool hasSkyVisibility = smi.HasSkyVisibility;
		var (flag, num) = smi.def.skyVisibilityInfo.GetVisibilityOf(smi.gameObject);
		smi.Internal_SetPercentClearSky(num);
		KSelectable component = smi.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !flag, smi);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, flag && num < 1f, smi);
		if (hasSkyVisibility != flag)
		{
			smi.TriggerVisibilityChange();
		}
	}
}
