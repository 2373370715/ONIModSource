using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E71 RID: 3697
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LogicWire")]
public class LogicWire : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IBitRating, IDisconnectable
{
	// Token: 0x06004A44 RID: 19012 RVA: 0x000D0145 File Offset: 0x000CE345
	public static int GetBitDepthAsInt(LogicWire.BitDepth rating)
	{
		if (rating == LogicWire.BitDepth.OneBit)
		{
			return 1;
		}
		if (rating != LogicWire.BitDepth.FourBit)
		{
			return 0;
		}
		return 4;
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x00259DF8 File Offset: 0x00257FF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.logicCircuitSystem.AddToNetworks(cell, this, false);
		base.Subscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
		base.Subscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
		this.Connect();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(LogicWire.OutlineSymbol, false);
	}

	// Token: 0x06004A46 RID: 19014 RVA: 0x00259E68 File Offset: 0x00258068
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.logicCircuitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06004A47 RID: 19015 RVA: 0x00259EF4 File Offset: 0x002580F4
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.logicCircuitSystem.GetNetworkForCell(cell) is LogicCircuitNetwork;
		}
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x000D0155 File Offset: 0x000CE355
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x00259F2C File Offset: 0x0025812C
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x00259F74 File Offset: 0x00258174
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x00259FC4 File Offset: 0x002581C4
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.logicCircuitSystem.GetConnections(cell, true);
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x00259FF4 File Offset: 0x002581F4
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.logicCircuitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x000D015D File Offset: 0x000CE35D
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x000D0165 File Offset: 0x000CE365
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x000D016E File Offset: 0x000CE36E
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06004A50 RID: 19024 RVA: 0x000D0184 File Offset: 0x000CE384
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

	// Token: 0x06004A51 RID: 19025 RVA: 0x000D0193 File Offset: 0x000CE393
	public LogicWire.BitDepth GetMaxBitRating()
	{
		return this.MaxBitDepth;
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x000D019B File Offset: 0x000CE39B
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x06004A53 RID: 19027 RVA: 0x0025A018 File Offset: 0x00258218
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x0025A054 File Offset: 0x00258254
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04003383 RID: 13187
	[SerializeField]
	public LogicWire.BitDepth MaxBitDepth;

	// Token: 0x04003384 RID: 13188
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04003385 RID: 13189
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x04003386 RID: 13190
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04003387 RID: 13191
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04003388 RID: 13192
	private System.Action firstFrameCallback;

	// Token: 0x02000E72 RID: 3698
	public enum BitDepth
	{
		// Token: 0x0400338A RID: 13194
		OneBit,
		// Token: 0x0400338B RID: 13195
		FourBit,
		// Token: 0x0400338C RID: 13196
		NumRatings
	}
}
