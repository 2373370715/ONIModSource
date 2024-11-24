using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000042 RID: 66
[AddComponentMenu("KMonoBehaviour/scripts/CargoBay")]
public class CargoBayConduit : KMonoBehaviour
{
	// Token: 0x06000122 RID: 290 RVA: 0x00142E60 File Offset: 0x00141060
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (CargoBayConduit.connectedPortStatus == null)
		{
			CargoBayConduit.connectedPortStatus = new StatusItem("CONNECTED_ROCKET_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null);
			CargoBayConduit.connectedWrongPortStatus = new StatusItem("CONNECTED_ROCKET_WRONG_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022, null);
			CargoBayConduit.connectedNoPortStatus = new StatusItem("CONNECTED_ROCKET_NO_PORT", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Bad, true, OverlayModes.None.ID, true, 129022, null);
		}
		if (base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad != null)
		{
			this.OnLaunchpadChainChanged(null);
			base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		}
		base.Subscribe<CargoBayConduit>(-1277991738, CargoBayConduit.OnLaunchDelegate);
		base.Subscribe<CargoBayConduit>(-887025858, CargoBayConduit.OnLandDelegate);
		this.storageType = base.GetComponent<CargoBay>().storageType;
		this.UpdateStatusItems();
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00142F74 File Offset: 0x00141174
	protected override void OnCleanUp()
	{
		LaunchPad currentPad = base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
		if (currentPad != null)
		{
			currentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00142FB8 File Offset: 0x001411B8
	public void OnLaunch(object data)
	{
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			component.conduitType = ConduitType.None;
		}
		base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00143004 File Offset: 0x00141204
	public void OnLand(object data)
	{
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			CargoBay.CargoType cargoType = this.storageType;
			if (cargoType != CargoBay.CargoType.Liquids)
			{
				if (cargoType == CargoBay.CargoType.Gasses)
				{
					component.conduitType = ConduitType.Gas;
				}
				else
				{
					component.conduitType = ConduitType.None;
				}
			}
			else
			{
				component.conduitType = ConduitType.Liquid;
			}
		}
		base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
		this.UpdateStatusItems();
	}

	// Token: 0x06000126 RID: 294 RVA: 0x000A6388 File Offset: 0x000A4588
	private void OnLaunchpadChainChanged(object data)
	{
		this.UpdateStatusItems();
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00143078 File Offset: 0x00141278
	private void UpdateStatusItems()
	{
		bool flag;
		bool flag2;
		this.HasMatchingConduitPort(out flag, out flag2);
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedPortStatus, this);
			return;
		}
		if (flag2)
		{
			this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedWrongPortStatus, this);
			return;
		}
		this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedNoPortStatus, this);
	}

	// Token: 0x06000128 RID: 296 RVA: 0x001430E8 File Offset: 0x001412E8
	private void HasMatchingConduitPort(out bool hasMatch, out bool hasAny)
	{
		hasMatch = false;
		hasAny = false;
		LaunchPad currentPad = base.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
		if (currentPad == null)
		{
			return;
		}
		ChainedBuilding.StatesInstance smi = currentPad.GetSMI<ChainedBuilding.StatesInstance>();
		if (smi == null)
		{
			return;
		}
		HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
		smi.GetLinkedBuildings(ref pooledHashSet);
		foreach (ChainedBuilding.StatesInstance statesInstance in pooledHashSet)
		{
			IConduitDispenser component = statesInstance.GetComponent<IConduitDispenser>();
			if (component != null)
			{
				hasAny = true;
				if (CargoBayConduit.ElementToCargoMap[component.ConduitType] == this.storageType)
				{
					hasMatch = true;
					break;
				}
			}
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x040000AF RID: 175
	public static Dictionary<ConduitType, CargoBay.CargoType> ElementToCargoMap = new Dictionary<ConduitType, CargoBay.CargoType>
	{
		{
			ConduitType.Solid,
			CargoBay.CargoType.Solids
		},
		{
			ConduitType.Liquid,
			CargoBay.CargoType.Liquids
		},
		{
			ConduitType.Gas,
			CargoBay.CargoType.Gasses
		}
	};

	// Token: 0x040000B0 RID: 176
	private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>(delegate(CargoBayConduit component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x040000B1 RID: 177
	private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>(delegate(CargoBayConduit component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x040000B2 RID: 178
	private static StatusItem connectedPortStatus;

	// Token: 0x040000B3 RID: 179
	private static StatusItem connectedWrongPortStatus;

	// Token: 0x040000B4 RID: 180
	private static StatusItem connectedNoPortStatus;

	// Token: 0x040000B5 RID: 181
	private CargoBay.CargoType storageType;

	// Token: 0x040000B6 RID: 182
	private Guid connectedConduitPortStatusItem;
}
