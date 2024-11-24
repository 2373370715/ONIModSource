using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000CF6 RID: 3318
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Conduit")]
public class Conduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IDisconnectable, FlowUtilityNetwork.IItem
{
	// Token: 0x060040AB RID: 16555 RVA: 0x000C9E20 File Offset: 0x000C8020
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x060040AC RID: 16556 RVA: 0x000C9E36 File Offset: 0x000C8036
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060040AD RID: 16557 RVA: 0x0023BEB8 File Offset: 0x0023A0B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Conduit>(-1201923725, Conduit.OnHighlightedDelegate);
		base.Subscribe<Conduit>(-700727624, Conduit.OnConduitFrozenDelegate);
		base.Subscribe<Conduit>(-1152799878, Conduit.OnConduitBoilingDelegate);
		base.Subscribe<Conduit>(-1555603773, Conduit.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x000C9E45 File Offset: 0x000C8045
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate);
		base.Subscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x060040AF RID: 16559 RVA: 0x0023BF10 File Offset: 0x0023A110
	protected virtual void OnStructureTemperatureRegistered(object data)
	{
		int cell = Grid.PosToCell(this);
		this.GetNetworkManager().AddToNetworks(cell, this, false);
		this.Connect();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Pipe, this);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().AddThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x0023BFA8 File Offset: 0x0023A1A8
	protected override void OnCleanUp()
	{
		base.Unsubscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate, false);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().RemoveThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
		}
		base.OnCleanUp();
	}

	// Token: 0x060040B1 RID: 16561 RVA: 0x000C9E6F File Offset: 0x000C806F
	protected ConduitFlowVisualizer GetFlowVisualizer()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidFlowVisualizer;
		}
		return Game.Instance.gasFlowVisualizer;
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x000C9E8F File Offset: 0x000C808F
	public IUtilityNetworkMgr GetNetworkManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x000C9EAF File Offset: 0x000C80AF
	public ConduitFlow GetFlowManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x000C9ECF File Offset: 0x000C80CF
	public static ConduitFlow GetFlowManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x060040B5 RID: 16565 RVA: 0x000C9EEA File Offset: 0x000C80EA
	public static IUtilityNetworkMgr GetNetworkManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x060040B6 RID: 16566 RVA: 0x0023C09C File Offset: 0x0023A29C
	public virtual void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x0023C0C8 File Offset: 0x0023A2C8
	public virtual bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		return networks.Contains(networkForCell);
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public virtual int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x060040B9 RID: 16569 RVA: 0x0023C0F0 File Offset: 0x0023A2F0
	private void OnHighlighted(object data)
	{
		int highlightedCell = ((bool)data) ? Grid.PosToCell(base.transform.GetPosition()) : -1;
		this.GetFlowVisualizer().SetHighlightedCell(highlightedCell);
	}

	// Token: 0x060040BA RID: 16570 RVA: 0x0023C128 File Offset: 0x0023A328
	private void OnConduitFrozen(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_FROZE,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_FROZE,
			takeDamageEffect = ((this.ConduitType == ConduitType.Gas) ? SpawnFXHashes.BuildingLeakLiquid : SpawnFXHashes.BuildingFreeze),
			fullDamageEffectName = ((this.ConduitType == ConduitType.Gas) ? "water_damage_kanim" : "ice_damage_kanim")
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x060040BB RID: 16571 RVA: 0x0023C1CC File Offset: 0x0023A3CC
	private void OnConduitBoiling(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_BOILED,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_BOILED,
			takeDamageEffect = SpawnFXHashes.BuildingLeakGas,
			fullDamageEffectName = "gas_damage_kanim"
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x060040BC RID: 16572 RVA: 0x000C9F05 File Offset: 0x000C8105
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x060040BD RID: 16573 RVA: 0x000C9F0D File Offset: 0x000C810D
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x060040BE RID: 16574 RVA: 0x000C9F16 File Offset: 0x000C8116
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x060040BF RID: 16575 RVA: 0x0023C250 File Offset: 0x0023A450
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			this.GetNetworkManager().ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x000C9F1E File Offset: 0x000C811E
	public void Disconnect()
	{
		this.disconnected = true;
		this.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x17000319 RID: 793
	// (set) Token: 0x060040C1 RID: 16577 RVA: 0x000A5E40 File Offset: 0x000A4040
	public FlowUtilityNetwork Network
	{
		set
		{
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x060040C2 RID: 16578 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public int Cell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x060040C3 RID: 16579 RVA: 0x000A6603 File Offset: 0x000A4803
	public Endpoint EndpointType
	{
		get
		{
			return Endpoint.Conduit;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x060040C4 RID: 16580 RVA: 0x000C9F32 File Offset: 0x000C8132
	public ConduitType ConduitType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x060040C5 RID: 16581 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x04002C3F RID: 11327
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04002C40 RID: 11328
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04002C41 RID: 11329
	public ConduitType type;

	// Token: 0x04002C42 RID: 11330
	private System.Action firstFrameCallback;

	// Token: 0x04002C43 RID: 11331
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnHighlightedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnHighlighted(data);
	});

	// Token: 0x04002C44 RID: 11332
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitFrozenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitFrozen(data);
	});

	// Token: 0x04002C45 RID: 11333
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitBoilingDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitBoiling(data);
	});

	// Token: 0x04002C46 RID: 11334
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x04002C47 RID: 11335
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04002C48 RID: 11336
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
