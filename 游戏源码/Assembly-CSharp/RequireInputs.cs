using System;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireInputs")]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
	[Flags]
	public enum Requirements
	{
		None = 0,
		NoWire = 1,
		NeedPower = 2,
		ConduitConnected = 4,
		ConduitEmpty = 8,
		AllPower = 3,
		AllConduit = 0xC,
		All = 0xF
	}

	[SerializeField]
	private bool requirePower = true;

	[SerializeField]
	private bool requireConduit;

	public bool requireConduitHasMass = true;

	public Requirements visualizeRequirements = Requirements.All;

	private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);

	private static readonly Operational.Flag pipesHaveMass = new Operational.Flag("pipesHaveMass", Operational.Flag.Type.Requirement);

	private Guid noWireStatusGuid;

	private Guid needPowerStatusGuid;

	private bool requirementsMet;

	private BuildingEnabledButton button;

	private IEnergyConsumer energy;

	public ConduitConsumer conduitConsumer;

	[MyCmpReq]
	private KSelectable selectable;

	[MyCmpGet]
	private Operational operational;

	private bool previouslyConnected = true;

	private bool previouslySatisfied = true;

	public bool RequiresPower => requirePower;

	public bool RequiresInputConduit => requireConduit;

	public bool RequirementsMet => requirementsMet;

	public void SetRequirements(bool power, bool conduit)
	{
		requirePower = power;
		requireConduit = conduit;
	}

	protected override void OnPrefabInit()
	{
		Bind();
	}

	protected override void OnSpawn()
	{
		CheckRequirements(forceEvent: true);
		Bind();
	}

	[ContextMenu("Bind")]
	private void Bind()
	{
		if (requirePower)
		{
			energy = GetComponent<IEnergyConsumer>();
			button = GetComponent<BuildingEnabledButton>();
		}
		if (requireConduit && !conduitConsumer)
		{
			conduitConsumer = GetComponent<ConduitConsumer>();
		}
	}

	public void Sim200ms(float dt)
	{
		CheckRequirements(forceEvent: false);
	}

	private void CheckRequirements(bool forceEvent)
	{
		bool flag = true;
		bool flag2 = false;
		if (requirePower)
		{
			bool isConnected = energy.IsConnected;
			bool isPowered = energy.IsPowered;
			flag = flag && isPowered && isConnected;
			bool show = VisualizeRequirement(Requirements.NeedPower) && isConnected && !isPowered && (button == null || button.IsEnabled);
			bool show2 = VisualizeRequirement(Requirements.NoWire) && !isConnected;
			needPowerStatusGuid = selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, needPowerStatusGuid, show, this);
			noWireStatusGuid = selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, noWireStatusGuid, show2, this);
			flag2 = flag != RequirementsMet && GetComponent<Light2D>() != null;
		}
		if (requireConduit)
		{
			bool flag3 = !conduitConsumer.enabled || conduitConsumer.IsConnected;
			bool flag4 = !conduitConsumer.enabled || conduitConsumer.IsSatisfied;
			if (VisualizeRequirement(Requirements.ConduitConnected) && previouslyConnected != flag3)
			{
				previouslyConnected = flag3;
				StatusItem statusItem = null;
				switch (conduitConsumer.TypeOfConduit)
				{
				case ConduitType.Liquid:
					statusItem = Db.Get().BuildingStatusItems.NeedLiquidIn;
					break;
				case ConduitType.Gas:
					statusItem = Db.Get().BuildingStatusItems.NeedGasIn;
					break;
				}
				if (statusItem != null)
				{
					selectable.ToggleStatusItem(statusItem, !flag3, new Tuple<ConduitType, Tag>(conduitConsumer.TypeOfConduit, conduitConsumer.capacityTag));
				}
				operational.SetFlag(inputConnectedFlag, flag3);
			}
			flag = flag && flag3;
			if (VisualizeRequirement(Requirements.ConduitEmpty) && previouslySatisfied != flag4)
			{
				previouslySatisfied = flag4;
				StatusItem statusItem2 = null;
				switch (conduitConsumer.TypeOfConduit)
				{
				case ConduitType.Liquid:
					statusItem2 = Db.Get().BuildingStatusItems.LiquidPipeEmpty;
					break;
				case ConduitType.Gas:
					statusItem2 = Db.Get().BuildingStatusItems.GasPipeEmpty;
					break;
				}
				if (requireConduitHasMass)
				{
					if (statusItem2 != null)
					{
						selectable.ToggleStatusItem(statusItem2, !flag4, this);
					}
					operational.SetFlag(pipesHaveMass, flag4);
				}
			}
		}
		requirementsMet = flag;
		if (flag2)
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			if (roomOfGameObject != null)
			{
				Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
			}
		}
	}

	public bool VisualizeRequirement(Requirements r)
	{
		return (visualizeRequirements & r) == r;
	}
}
