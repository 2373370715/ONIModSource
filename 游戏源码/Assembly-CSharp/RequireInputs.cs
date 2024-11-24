using System;
using UnityEngine;

// Token: 0x02001798 RID: 6040
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireInputs")]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x06007C59 RID: 31833 RVA: 0x000F1E65 File Offset: 0x000F0065
	public bool RequiresPower
	{
		get
		{
			return this.requirePower;
		}
	}

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x06007C5A RID: 31834 RVA: 0x000F1E6D File Offset: 0x000F006D
	public bool RequiresInputConduit
	{
		get
		{
			return this.requireConduit;
		}
	}

	// Token: 0x06007C5B RID: 31835 RVA: 0x000F1E75 File Offset: 0x000F0075
	public void SetRequirements(bool power, bool conduit)
	{
		this.requirePower = power;
		this.requireConduit = conduit;
	}

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x06007C5C RID: 31836 RVA: 0x000F1E85 File Offset: 0x000F0085
	public bool RequirementsMet
	{
		get
		{
			return this.requirementsMet;
		}
	}

	// Token: 0x06007C5D RID: 31837 RVA: 0x000F1E8D File Offset: 0x000F008D
	protected override void OnPrefabInit()
	{
		this.Bind();
	}

	// Token: 0x06007C5E RID: 31838 RVA: 0x000F1E95 File Offset: 0x000F0095
	protected override void OnSpawn()
	{
		this.CheckRequirements(true);
		this.Bind();
	}

	// Token: 0x06007C5F RID: 31839 RVA: 0x00320C34 File Offset: 0x0031EE34
	[ContextMenu("Bind")]
	private void Bind()
	{
		if (this.requirePower)
		{
			this.energy = base.GetComponent<IEnergyConsumer>();
			this.button = base.GetComponent<BuildingEnabledButton>();
		}
		if (this.requireConduit && !this.conduitConsumer)
		{
			this.conduitConsumer = base.GetComponent<ConduitConsumer>();
		}
	}

	// Token: 0x06007C60 RID: 31840 RVA: 0x000F1EA4 File Offset: 0x000F00A4
	public void Sim200ms(float dt)
	{
		this.CheckRequirements(false);
	}

	// Token: 0x06007C61 RID: 31841 RVA: 0x00320C84 File Offset: 0x0031EE84
	private void CheckRequirements(bool forceEvent)
	{
		bool flag = true;
		bool flag2 = false;
		if (this.requirePower)
		{
			bool isConnected = this.energy.IsConnected;
			bool isPowered = this.energy.IsPowered;
			flag = (flag && isPowered && isConnected);
			bool show = this.VisualizeRequirement(RequireInputs.Requirements.NeedPower) && isConnected && !isPowered && (this.button == null || this.button.IsEnabled);
			bool show2 = this.VisualizeRequirement(RequireInputs.Requirements.NoWire) && !isConnected;
			this.needPowerStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, this.needPowerStatusGuid, show, this);
			this.noWireStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, this.noWireStatusGuid, show2, this);
			flag2 = (flag != this.RequirementsMet && base.GetComponent<Light2D>() != null);
		}
		if (this.requireConduit)
		{
			bool flag3 = !this.conduitConsumer.enabled || this.conduitConsumer.IsConnected;
			bool flag4 = !this.conduitConsumer.enabled || this.conduitConsumer.IsSatisfied;
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected) && this.previouslyConnected != flag3)
			{
				this.previouslyConnected = flag3;
				StatusItem statusItem = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem = Db.Get().BuildingStatusItems.NeedLiquidIn;
					}
				}
				else
				{
					statusItem = Db.Get().BuildingStatusItems.NeedGasIn;
				}
				if (statusItem != null)
				{
					this.selectable.ToggleStatusItem(statusItem, !flag3, new global::Tuple<ConduitType, Tag>(this.conduitConsumer.TypeOfConduit, this.conduitConsumer.capacityTag));
				}
				this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag3);
			}
			flag = (flag && flag3);
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty) && this.previouslySatisfied != flag4)
			{
				this.previouslySatisfied = flag4;
				StatusItem statusItem2 = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem2 = Db.Get().BuildingStatusItems.LiquidPipeEmpty;
					}
				}
				else
				{
					statusItem2 = Db.Get().BuildingStatusItems.GasPipeEmpty;
				}
				if (this.requireConduitHasMass)
				{
					if (statusItem2 != null)
					{
						this.selectable.ToggleStatusItem(statusItem2, !flag4, this);
					}
					this.operational.SetFlag(RequireInputs.pipesHaveMass, flag4);
				}
			}
		}
		this.requirementsMet = flag;
		if (flag2)
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			if (roomOfGameObject != null)
			{
				Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
			}
		}
	}

	// Token: 0x06007C62 RID: 31842 RVA: 0x000F1EAD File Offset: 0x000F00AD
	public bool VisualizeRequirement(RequireInputs.Requirements r)
	{
		return (this.visualizeRequirements & r) == r;
	}

	// Token: 0x04005E0A RID: 24074
	[SerializeField]
	private bool requirePower = true;

	// Token: 0x04005E0B RID: 24075
	[SerializeField]
	private bool requireConduit;

	// Token: 0x04005E0C RID: 24076
	public bool requireConduitHasMass = true;

	// Token: 0x04005E0D RID: 24077
	public RequireInputs.Requirements visualizeRequirements = RequireInputs.Requirements.All;

	// Token: 0x04005E0E RID: 24078
	private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04005E0F RID: 24079
	private static readonly Operational.Flag pipesHaveMass = new Operational.Flag("pipesHaveMass", Operational.Flag.Type.Requirement);

	// Token: 0x04005E10 RID: 24080
	private Guid noWireStatusGuid;

	// Token: 0x04005E11 RID: 24081
	private Guid needPowerStatusGuid;

	// Token: 0x04005E12 RID: 24082
	private bool requirementsMet;

	// Token: 0x04005E13 RID: 24083
	private BuildingEnabledButton button;

	// Token: 0x04005E14 RID: 24084
	private IEnergyConsumer energy;

	// Token: 0x04005E15 RID: 24085
	public ConduitConsumer conduitConsumer;

	// Token: 0x04005E16 RID: 24086
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005E17 RID: 24087
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005E18 RID: 24088
	private bool previouslyConnected = true;

	// Token: 0x04005E19 RID: 24089
	private bool previouslySatisfied = true;

	// Token: 0x02001799 RID: 6041
	[Flags]
	public enum Requirements
	{
		// Token: 0x04005E1B RID: 24091
		None = 0,
		// Token: 0x04005E1C RID: 24092
		NoWire = 1,
		// Token: 0x04005E1D RID: 24093
		NeedPower = 2,
		// Token: 0x04005E1E RID: 24094
		ConduitConnected = 4,
		// Token: 0x04005E1F RID: 24095
		ConduitEmpty = 8,
		// Token: 0x04005E20 RID: 24096
		AllPower = 3,
		// Token: 0x04005E21 RID: 24097
		AllConduit = 12,
		// Token: 0x04005E22 RID: 24098
		All = 15
	}
}
