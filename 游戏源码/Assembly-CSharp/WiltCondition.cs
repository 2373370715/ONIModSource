using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020011F6 RID: 4598
[AddComponentMenu("KMonoBehaviour/scripts/WiltCondition")]
public class WiltCondition : KMonoBehaviour
{
	// Token: 0x06005D93 RID: 23955 RVA: 0x000DD2C9 File Offset: 0x000DB4C9
	public bool IsWilting()
	{
		return this.wilting;
	}

	// Token: 0x06005D94 RID: 23956 RVA: 0x0029E978 File Offset: 0x0029CB78
	public List<WiltCondition.Condition> CurrentWiltSources()
	{
		List<WiltCondition.Condition> list = new List<WiltCondition.Condition>();
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				list.Add((WiltCondition.Condition)keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06005D95 RID: 23957 RVA: 0x0029E9E4 File Offset: 0x0029CBE4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.WiltConditions.Add(0, true);
		this.WiltConditions.Add(1, true);
		this.WiltConditions.Add(2, true);
		this.WiltConditions.Add(3, true);
		this.WiltConditions.Add(4, true);
		this.WiltConditions.Add(5, true);
		this.WiltConditions.Add(6, true);
		this.WiltConditions.Add(7, true);
		this.WiltConditions.Add(9, true);
		this.WiltConditions.Add(10, true);
		this.WiltConditions.Add(11, true);
		this.WiltConditions.Add(12, true);
		base.Subscribe<WiltCondition>(-107174716, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1758196852, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1234705021, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-55477301, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(115888613, WiltCondition.SetTemperatureTrueDelegate);
		base.Subscribe<WiltCondition>(-593125877, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-1175525437, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-907106982, WiltCondition.SetPressureTrueDelegate);
		base.Subscribe<WiltCondition>(103243573, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(646131325, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(221594799, WiltCondition.SetAtmosphereElementFalseDelegate);
		base.Subscribe<WiltCondition>(777259436, WiltCondition.SetAtmosphereElementTrueDelegate);
		base.Subscribe<WiltCondition>(1949704522, WiltCondition.SetDrowningFalseDelegate);
		base.Subscribe<WiltCondition>(99949694, WiltCondition.SetDrowningTrueDelegate);
		base.Subscribe<WiltCondition>(-2057657673, WiltCondition.SetDryingOutFalseDelegate);
		base.Subscribe<WiltCondition>(1555379996, WiltCondition.SetDryingOutTrueDelegate);
		base.Subscribe<WiltCondition>(-370379773, WiltCondition.SetIrrigationFalseDelegate);
		base.Subscribe<WiltCondition>(207387507, WiltCondition.SetIrrigationTrueDelegate);
		base.Subscribe<WiltCondition>(-1073674739, WiltCondition.SetFertilizedFalseDelegate);
		base.Subscribe<WiltCondition>(-1396791468, WiltCondition.SetFertilizedTrueDelegate);
		base.Subscribe<WiltCondition>(1113102781, WiltCondition.SetIlluminationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1387626797, WiltCondition.SetIlluminationComfortFalseDelegate);
		base.Subscribe<WiltCondition>(1628751838, WiltCondition.SetReceptacleTrueDelegate);
		base.Subscribe<WiltCondition>(960378201, WiltCondition.SetReceptacleFalseDelegate);
		base.Subscribe<WiltCondition>(-1089732772, WiltCondition.SetEntombedDelegate);
		base.Subscribe<WiltCondition>(912965142, WiltCondition.SetRootHealthDelegate);
		base.Subscribe<WiltCondition>(874353739, WiltCondition.SetRadiationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1788072223, WiltCondition.SetRadiationComfortFalseDelegate);
	}

	// Token: 0x06005D96 RID: 23958 RVA: 0x0029EC74 File Offset: 0x0029CE74
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CheckShouldWilt();
		if (this.wilting)
		{
			this.DoWilt();
			if (!this.goingToWilt)
			{
				this.goingToWilt = true;
				this.Recover();
				return;
			}
		}
		else
		{
			this.DoRecover();
			if (this.goingToWilt)
			{
				this.goingToWilt = false;
				this.Wilt();
			}
		}
	}

	// Token: 0x06005D97 RID: 23959 RVA: 0x000DD2D1 File Offset: 0x000DB4D1
	protected override void OnCleanUp()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		this.recoverSchedulerHandler.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005D98 RID: 23960 RVA: 0x000DD2EF File Offset: 0x000DB4EF
	private void SetCondition(WiltCondition.Condition condition, bool satisfiedState)
	{
		if (!this.WiltConditions.ContainsKey((int)condition))
		{
			return;
		}
		this.WiltConditions[(int)condition] = satisfiedState;
		this.CheckShouldWilt();
	}

	// Token: 0x06005D99 RID: 23961 RVA: 0x0029ECCC File Offset: 0x0029CECC
	private void CheckShouldWilt()
	{
		bool flag = false;
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (!this.goingToWilt)
			{
				this.Wilt();
				return;
			}
		}
		else if (this.goingToWilt)
		{
			this.Recover();
		}
	}

	// Token: 0x06005D9A RID: 23962 RVA: 0x0029ED48 File Offset: 0x0029CF48
	private void Wilt()
	{
		if (!this.goingToWilt)
		{
			this.goingToWilt = true;
			this.recoverSchedulerHandler.ClearScheduler();
			if (!this.wiltSchedulerHandler.IsValid)
			{
				this.wiltSchedulerHandler = GameScheduler.Instance.Schedule("Wilt", this.WiltDelay, new Action<object>(WiltCondition.DoWiltCallback), this, null);
			}
		}
	}

	// Token: 0x06005D9B RID: 23963 RVA: 0x0029EDA8 File Offset: 0x0029CFA8
	private void Recover()
	{
		if (this.goingToWilt)
		{
			this.goingToWilt = false;
			this.wiltSchedulerHandler.ClearScheduler();
			if (!this.recoverSchedulerHandler.IsValid)
			{
				this.recoverSchedulerHandler = GameScheduler.Instance.Schedule("Recover", this.RecoveryDelay, new Action<object>(WiltCondition.DoRecoverCallback), this, null);
			}
		}
	}

	// Token: 0x06005D9C RID: 23964 RVA: 0x000DD313 File Offset: 0x000DB513
	private static void DoWiltCallback(object data)
	{
		((WiltCondition)data).DoWilt();
	}

	// Token: 0x06005D9D RID: 23965 RVA: 0x0029EE08 File Offset: 0x0029D008
	private void DoWilt()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		component.GetComponent<KPrefabID>().AddTag(GameTags.Wilting, false);
		if (!this.wilting)
		{
			this.wilting = true;
			base.Trigger(-724860998, null);
		}
		if (this.rm != null)
		{
			if (this.rm.Replanted)
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, base.GetComponent<ReceptacleMonitor>());
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.Wilting, base.GetComponent<ReceptacleMonitor>());
			return;
		}
		else
		{
			ReceptacleMonitor.StatesInstance smi = component.GetSMI<ReceptacleMonitor.StatesInstance>();
			if (smi != null && !smi.IsInsideState(smi.sm.wild))
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, this);
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, this);
			return;
		}
	}

	// Token: 0x06005D9E RID: 23966 RVA: 0x0029EEF4 File Offset: 0x0029D0F4
	public string WiltCausesString()
	{
		string text = "";
		List<IWiltCause> allSMI = this.GetAllSMI<IWiltCause>();
		allSMI.AddRange(base.GetComponents<IWiltCause>());
		foreach (IWiltCause wiltCause in allSMI)
		{
			foreach (WiltCondition.Condition key in wiltCause.Conditions)
			{
				if (this.WiltConditions.ContainsKey((int)key) && !this.WiltConditions[(int)key])
				{
					text += "\n";
					text += wiltCause.WiltStateString;
					break;
				}
			}
		}
		return text;
	}

	// Token: 0x06005D9F RID: 23967 RVA: 0x000DD320 File Offset: 0x000DB520
	private static void DoRecoverCallback(object data)
	{
		((WiltCondition)data).DoRecover();
	}

	// Token: 0x06005DA0 RID: 23968 RVA: 0x0029EFAC File Offset: 0x0029D1AC
	private void DoRecover()
	{
		this.recoverSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		this.wilting = false;
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.Wilting, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, false);
		component.GetComponent<KPrefabID>().RemoveTag(GameTags.Wilting);
		base.Trigger(712767498, null);
	}

	// Token: 0x04004249 RID: 16969
	[MyCmpGet]
	private ReceptacleMonitor rm;

	// Token: 0x0400424A RID: 16970
	[Serialize]
	private bool goingToWilt;

	// Token: 0x0400424B RID: 16971
	[Serialize]
	private bool wilting;

	// Token: 0x0400424C RID: 16972
	private Dictionary<int, bool> WiltConditions = new Dictionary<int, bool>();

	// Token: 0x0400424D RID: 16973
	public float WiltDelay = 1f;

	// Token: 0x0400424E RID: 16974
	public float RecoveryDelay = 1f;

	// Token: 0x0400424F RID: 16975
	private SchedulerHandle wiltSchedulerHandler;

	// Token: 0x04004250 RID: 16976
	private SchedulerHandle recoverSchedulerHandler;

	// Token: 0x04004251 RID: 16977
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, false);
	});

	// Token: 0x04004252 RID: 16978
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, true);
	});

	// Token: 0x04004253 RID: 16979
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, false);
	});

	// Token: 0x04004254 RID: 16980
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, true);
	});

	// Token: 0x04004255 RID: 16981
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, false);
	});

	// Token: 0x04004256 RID: 16982
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, true);
	});

	// Token: 0x04004257 RID: 16983
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, false);
	});

	// Token: 0x04004258 RID: 16984
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, true);
	});

	// Token: 0x04004259 RID: 16985
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, false);
	});

	// Token: 0x0400425A RID: 16986
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, true);
	});

	// Token: 0x0400425B RID: 16987
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, false);
	});

	// Token: 0x0400425C RID: 16988
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, true);
	});

	// Token: 0x0400425D RID: 16989
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, false);
	});

	// Token: 0x0400425E RID: 16990
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, true);
	});

	// Token: 0x0400425F RID: 16991
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, false);
	});

	// Token: 0x04004260 RID: 16992
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, true);
	});

	// Token: 0x04004261 RID: 16993
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, false);
	});

	// Token: 0x04004262 RID: 16994
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, true);
	});

	// Token: 0x04004263 RID: 16995
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetEntombedDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Entombed, !(bool)data);
	});

	// Token: 0x04004264 RID: 16996
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRootHealthDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.UnhealthyRoot, (bool)data);
	});

	// Token: 0x04004265 RID: 16997
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, false);
	});

	// Token: 0x04004266 RID: 16998
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, true);
	});

	// Token: 0x020011F7 RID: 4599
	public enum Condition
	{
		// Token: 0x04004268 RID: 17000
		Temperature,
		// Token: 0x04004269 RID: 17001
		Pressure,
		// Token: 0x0400426A RID: 17002
		AtmosphereElement,
		// Token: 0x0400426B RID: 17003
		Drowning,
		// Token: 0x0400426C RID: 17004
		Fertilized,
		// Token: 0x0400426D RID: 17005
		DryingOut,
		// Token: 0x0400426E RID: 17006
		Irrigation,
		// Token: 0x0400426F RID: 17007
		IlluminationComfort,
		// Token: 0x04004270 RID: 17008
		Darkness,
		// Token: 0x04004271 RID: 17009
		Receptacle,
		// Token: 0x04004272 RID: 17010
		Entombed,
		// Token: 0x04004273 RID: 17011
		UnhealthyRoot,
		// Token: 0x04004274 RID: 17012
		Radiation,
		// Token: 0x04004275 RID: 17013
		Count
	}
}
