﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Wire")]
public class Wire : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	public static float GetMaxWattageAsFloat(Wire.WattageRating rating)
	{
		switch (rating)
		{
		case Wire.WattageRating.Max500:
			return 500f;
		case Wire.WattageRating.Max1000:
			return 1000f;
		case Wire.WattageRating.Max2000:
			return 2000f;
		case Wire.WattageRating.Max20000:
			return 20000f;
		case Wire.WattageRating.Max50000:
			return 50000f;
		default:
			return 0f;
		}
	}

		public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) is ElectricalUtilityNetwork;
		}
	}

		public ushort NetworkID
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) as ElectricalUtilityNetwork;
			if (electricalUtilityNetwork == null)
			{
				return ushort.MaxValue;
			}
			return (ushort)electricalUtilityNetwork.id;
		}
	}

	protected override void OnSpawn()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.electricalConduitSystem.AddToNetworks(cell, this, false);
		this.InitializeSwitchState();
		base.Subscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate);
		base.Subscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireCircuitStatus, this);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireMaxWattageStatus, this);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Wire.OutlineSymbol, false);
	}

	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.electricalConduitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	private void InitializeSwitchState()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		bool flag = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			CircuitSwitch component = gameObject.GetComponent<CircuitSwitch>();
			if (component != null)
			{
				flag = true;
				component.AttachWire(this);
			}
		}
		if (!flag)
		{
			this.Connect();
		}
	}

	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.electricalConduitSystem.GetConnections(cell, true);
	}

	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.electricalConduitSystem.GetVisualizerString(wireConnections);
	}

	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	private void OnBuildingFullyRepaired(object data)
	{
		this.InitializeSwitchState();
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.Wires, false);
		if (Wire.WireCircuitStatus == null)
		{
			Wire.WireCircuitStatus = new StatusItem("WireCircuitStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitID);
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				string wireLoadColor = GameUtil.GetWireLoadColor(wattsUsedByCircuit, maxWattageAsFloat, wattsNeededWhenActive);
				str = str.Replace("{CurrentLoadAndColor}", (wireLoadColor == Color.white.ToHexString()) ? GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true) : string.Concat(new string[]
				{
					"<color=#",
					wireLoadColor,
					">",
					GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true),
					"</color>"
				}));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				str = str.Replace("{WireType}", this.GetProperName());
				return str;
			});
		}
		if (Wire.WireMaxWattageStatus == null)
		{
			Wire.WireMaxWattageStatus = new StatusItem("WireMaxWattageStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				str = str.Replace("{TotalPotentialLoadAndColor}", (wattsNeededWhenActive > maxWattageAsFloat) ? string.Concat(new string[]
				{
					"<color=#",
					new Color(0.9843137f, 0.6901961f, 0.23137255f).ToHexString(),
					">",
					GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true),
					"</color>"
				}) : GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				return str;
			});
		}
	}

	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.MaxWattageRating;
	}

	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
	}

	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

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

	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	[SerializeField]
	public Wire.WattageRating MaxWattageRating;

	[SerializeField]
	private bool disconnected = true;

	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	public float circuitOverloadTime;

	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	private static StatusItem WireCircuitStatus = null;

	private static StatusItem WireMaxWattageStatus = null;

	private System.Action firstFrameCallback;

	public enum WattageRating
	{
		Max500,
		Max1000,
		Max2000,
		Max20000,
		Max50000,
		NumRatings
	}
}
