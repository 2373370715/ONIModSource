using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D03 RID: 3331
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class ConduitThresholdSensor : ConduitSensor
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06004103 RID: 16643
	public abstract float CurrentValue { get; }

	// Token: 0x06004104 RID: 16644 RVA: 0x000CA13C File Offset: 0x000C833C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ConduitThresholdSensor>(-905833192, ConduitThresholdSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x0023CB3C File Offset: 0x0023AD3C
	private void OnCopySettings(object data)
	{
		ConduitThresholdSensor component = ((GameObject)data).GetComponent<ConduitThresholdSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x0023CB78 File Offset: 0x0023AD78
	protected override void ConduitUpdate(float dt)
	{
		if (this.GetContainedMass() <= 0f && !this.dirty)
		{
			return;
		}
		float currentValue = this.CurrentValue;
		this.dirty = false;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004107 RID: 16647 RVA: 0x0023CC04 File Offset: 0x0023AE04
	private float GetContainedMass()
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			return Conduit.GetFlowManager(this.conduitType).GetContents(cell).mass;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents.pickupableHandle);
		if (pickupable != null)
		{
			return pickupable.PrimaryElement.Mass;
		}
		return 0f;
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06004108 RID: 16648 RVA: 0x000CA155 File Offset: 0x000C8355
	// (set) Token: 0x06004109 RID: 16649 RVA: 0x000CA15D File Offset: 0x000C835D
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x0600410A RID: 16650 RVA: 0x000CA16D File Offset: 0x000C836D
	// (set) Token: 0x0600410B RID: 16651 RVA: 0x000CA175 File Offset: 0x000C8375
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x04002C64 RID: 11364
	[SerializeField]
	[Serialize]
	protected float threshold;

	// Token: 0x04002C65 RID: 11365
	[SerializeField]
	[Serialize]
	protected bool activateAboveThreshold = true;

	// Token: 0x04002C66 RID: 11366
	[Serialize]
	private bool dirty = true;

	// Token: 0x04002C67 RID: 11367
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002C68 RID: 11368
	private static readonly EventSystem.IntraObjectHandler<ConduitThresholdSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ConduitThresholdSensor>(delegate(ConduitThresholdSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
