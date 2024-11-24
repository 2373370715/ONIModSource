using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000AB5 RID: 2741
[AddComponentMenu("KMonoBehaviour/scripts/Operational")]
public class Operational : KMonoBehaviour
{
	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06003304 RID: 13060 RVA: 0x000C1483 File Offset: 0x000BF683
	// (set) Token: 0x06003305 RID: 13061 RVA: 0x000C148B File Offset: 0x000BF68B
	public bool IsFunctional { get; private set; }

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06003306 RID: 13062 RVA: 0x000C1494 File Offset: 0x000BF694
	// (set) Token: 0x06003307 RID: 13063 RVA: 0x000C149C File Offset: 0x000BF69C
	public bool IsOperational { get; private set; }

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06003308 RID: 13064 RVA: 0x000C14A5 File Offset: 0x000BF6A5
	// (set) Token: 0x06003309 RID: 13065 RVA: 0x000C14AD File Offset: 0x000BF6AD
	public bool IsActive { get; private set; }

	// Token: 0x0600330A RID: 13066 RVA: 0x000C14B6 File Offset: 0x000BF6B6
	[OnSerializing]
	private void OnSerializing()
	{
		this.AddTimeData(this.IsActive);
		this.activeStartTime = GameClock.Instance.GetTime();
		this.inactiveStartTime = GameClock.Instance.GetTime();
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x000C14E4 File Offset: 0x000BF6E4
	protected override void OnPrefabInit()
	{
		this.UpdateFunctional();
		this.UpdateOperational();
		base.Subscribe<Operational>(-1661515756, Operational.OnNewBuildingDelegate);
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x00204DAC File Offset: 0x00202FAC
	public void OnNewBuilding(object data)
	{
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.creationTime > 0f)
		{
			this.inactiveStartTime = component.creationTime;
			this.activeStartTime = component.creationTime;
		}
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x000C151F File Offset: 0x000BF71F
	public bool IsOperationalType(Operational.Flag.Type type)
	{
		if (type == Operational.Flag.Type.Functional)
		{
			return this.IsFunctional;
		}
		return this.IsOperational;
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x00204DE8 File Offset: 0x00202FE8
	public void SetFlag(Operational.Flag flag, bool value)
	{
		bool flag2 = false;
		if (this.Flags.TryGetValue(flag, out flag2))
		{
			if (flag2 != value)
			{
				this.Flags[flag] = value;
				base.Trigger(187661686, flag);
			}
		}
		else
		{
			this.Flags[flag] = value;
			base.Trigger(187661686, flag);
		}
		if (flag.FlagType == Operational.Flag.Type.Functional && value != this.IsFunctional)
		{
			this.UpdateFunctional();
		}
		if (value != this.IsOperational)
		{
			this.UpdateOperational();
		}
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x00204E68 File Offset: 0x00203068
	public bool GetFlag(Operational.Flag flag)
	{
		bool result = false;
		this.Flags.TryGetValue(flag, out result);
		return result;
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x00204E88 File Offset: 0x00203088
	private void UpdateFunctional()
	{
		bool isFunctional = true;
		foreach (KeyValuePair<Operational.Flag, bool> keyValuePair in this.Flags)
		{
			if (keyValuePair.Key.FlagType == Operational.Flag.Type.Functional && !keyValuePair.Value)
			{
				isFunctional = false;
				break;
			}
		}
		this.IsFunctional = isFunctional;
		base.Trigger(-1852328367, this.IsFunctional);
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x00204F10 File Offset: 0x00203110
	private void UpdateOperational()
	{
		Dictionary<Operational.Flag, bool>.Enumerator enumerator = this.Flags.GetEnumerator();
		bool flag = true;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Operational.Flag, bool> keyValuePair = enumerator.Current;
			if (!keyValuePair.Value)
			{
				flag = false;
				break;
			}
		}
		if (flag != this.IsOperational)
		{
			this.IsOperational = flag;
			if (!this.IsOperational)
			{
				this.SetActive(false, false);
			}
			if (this.IsOperational)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Operational, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Operational);
			}
			base.Trigger(-592767678, this.IsOperational);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x000C1532 File Offset: 0x000BF732
	public void SetActive(bool value, bool force_ignore = false)
	{
		if (this.IsActive != value)
		{
			this.AddTimeData(value);
			base.Trigger(824508782, this);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x00204FC4 File Offset: 0x002031C4
	private void AddTimeData(bool value)
	{
		float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
		float time = GameClock.Instance.GetTime();
		float num2 = time - num;
		if (this.IsActive)
		{
			this.activeTime += num2;
		}
		else
		{
			this.inactiveTime += num2;
		}
		this.IsActive = value;
		if (this.IsActive)
		{
			this.activeStartTime = time;
			return;
		}
		this.inactiveStartTime = time;
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x0020503C File Offset: 0x0020323C
	public void OnNewDay(object data)
	{
		this.AddTimeData(this.IsActive);
		this.uptimeData.Add(this.activeTime / 600f);
		while (this.uptimeData.Count > this.MAX_DATA_POINTS)
		{
			this.uptimeData.RemoveAt(0);
		}
		this.activeTime = 0f;
		this.inactiveTime = 0f;
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x002050A4 File Offset: 0x002032A4
	public float GetCurrentCycleUptime()
	{
		if (this.IsActive)
		{
			float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
			float num2 = GameClock.Instance.GetTime() - num;
			return (this.activeTime + num2) / GameClock.Instance.GetTimeSinceStartOfCycle();
		}
		return this.activeTime / GameClock.Instance.GetTimeSinceStartOfCycle();
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x000C1565 File Offset: 0x000BF765
	public float GetLastCycleUptime()
	{
		if (this.uptimeData.Count > 0)
		{
			return this.uptimeData[this.uptimeData.Count - 1];
		}
		return 0f;
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x00205104 File Offset: 0x00203304
	public float GetUptimeOverCycles(int num_cycles)
	{
		if (this.uptimeData.Count > 0)
		{
			int num = Mathf.Min(this.uptimeData.Count, num_cycles);
			float num2 = 0f;
			for (int i = num - 1; i >= 0; i--)
			{
				num2 += this.uptimeData[i];
			}
			return num2 / (float)num;
		}
		return 0f;
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x000C1593 File Offset: 0x000BF793
	public bool MeetsRequirements(Operational.State stateRequirement)
	{
		switch (stateRequirement)
		{
		case Operational.State.Operational:
			return this.IsOperational;
		case Operational.State.Functional:
			return this.IsFunctional;
		case Operational.State.Active:
			return this.IsActive;
		}
		return true;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x000C15C3 File Offset: 0x000BF7C3
	public static GameHashes GetEventForState(Operational.State state)
	{
		if (state == Operational.State.Operational)
		{
			return GameHashes.OperationalChanged;
		}
		if (state == Operational.State.Functional)
		{
			return GameHashes.FunctionalChanged;
		}
		return GameHashes.ActiveChanged;
	}

	// Token: 0x0400225D RID: 8797
	[Serialize]
	public float inactiveStartTime;

	// Token: 0x0400225E RID: 8798
	[Serialize]
	public float activeStartTime;

	// Token: 0x0400225F RID: 8799
	[Serialize]
	private List<float> uptimeData = new List<float>();

	// Token: 0x04002260 RID: 8800
	[Serialize]
	private float activeTime;

	// Token: 0x04002261 RID: 8801
	[Serialize]
	private float inactiveTime;

	// Token: 0x04002262 RID: 8802
	private int MAX_DATA_POINTS = 5;

	// Token: 0x04002263 RID: 8803
	public Dictionary<Operational.Flag, bool> Flags = new Dictionary<Operational.Flag, bool>();

	// Token: 0x04002264 RID: 8804
	private static readonly EventSystem.IntraObjectHandler<Operational> OnNewBuildingDelegate = new EventSystem.IntraObjectHandler<Operational>(delegate(Operational component, object data)
	{
		component.OnNewBuilding(data);
	});

	// Token: 0x02000AB6 RID: 2742
	public enum State
	{
		// Token: 0x04002266 RID: 8806
		Operational,
		// Token: 0x04002267 RID: 8807
		Functional,
		// Token: 0x04002268 RID: 8808
		Active,
		// Token: 0x04002269 RID: 8809
		None
	}

	// Token: 0x02000AB7 RID: 2743
	public class Flag
	{
		// Token: 0x0600331C RID: 13084 RVA: 0x000C161E File Offset: 0x000BF81E
		public Flag(string name, Operational.Flag.Type type)
		{
			this.Name = name;
			this.FlagType = type;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000C1634 File Offset: 0x000BF834
		public static Operational.Flag.Type GetFlagType(Operational.State operationalState)
		{
			switch (operationalState)
			{
			case Operational.State.Operational:
			case Operational.State.Active:
				return Operational.Flag.Type.Requirement;
			case Operational.State.Functional:
				return Operational.Flag.Type.Functional;
			}
			throw new InvalidOperationException("Can not convert NONE state to an Operational Flag Type");
		}

		// Token: 0x0400226A RID: 8810
		public string Name;

		// Token: 0x0400226B RID: 8811
		public Operational.Flag.Type FlagType;

		// Token: 0x02000AB8 RID: 2744
		public enum Type
		{
			// Token: 0x0400226D RID: 8813
			Requirement,
			// Token: 0x0400226E RID: 8814
			Functional
		}
	}
}
