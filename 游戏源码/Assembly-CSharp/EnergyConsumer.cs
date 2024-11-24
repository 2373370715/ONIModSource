using System;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x02001291 RID: 4753
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
[AddComponentMenu("KMonoBehaviour/scripts/EnergyConsumer")]
public class EnergyConsumer : KMonoBehaviour, ISaveLoadable, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor
{
	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x060061A6 RID: 24998 RVA: 0x000DFB4B File Offset: 0x000DDD4B
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x060061A7 RID: 24999 RVA: 0x000DFB53 File Offset: 0x000DDD53
	// (set) Token: 0x060061A8 RID: 25000 RVA: 0x000DFB5B File Offset: 0x000DDD5B
	public int PowerCell { get; private set; }

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x060061A9 RID: 25001 RVA: 0x000DFB64 File Offset: 0x000DDD64
	public bool HasWire
	{
		get
		{
			return Grid.Objects[this.PowerCell, 26] != null;
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x060061AA RID: 25002 RVA: 0x000DFB7E File Offset: 0x000DDD7E
	// (set) Token: 0x060061AB RID: 25003 RVA: 0x000DFB90 File Offset: 0x000DDD90
	public virtual bool IsPowered
	{
		get
		{
			return this.operational.GetFlag(EnergyConsumer.PoweredFlag);
		}
		protected set
		{
			this.operational.SetFlag(EnergyConsumer.PoweredFlag, value);
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x060061AC RID: 25004 RVA: 0x000DFBA3 File Offset: 0x000DDDA3
	public bool IsConnected
	{
		get
		{
			return this.CircuitID != ushort.MaxValue;
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x060061AD RID: 25005 RVA: 0x000DFBB5 File Offset: 0x000DDDB5
	public string Name
	{
		get
		{
			return this.selectable.GetName();
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x060061AE RID: 25006 RVA: 0x000DFBC2 File Offset: 0x000DDDC2
	// (set) Token: 0x060061AF RID: 25007 RVA: 0x000DFBCA File Offset: 0x000DDDCA
	public bool IsVirtual { get; private set; }

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x060061B0 RID: 25008 RVA: 0x000DFBD3 File Offset: 0x000DDDD3
	// (set) Token: 0x060061B1 RID: 25009 RVA: 0x000DFBDB File Offset: 0x000DDDDB
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x060061B2 RID: 25010 RVA: 0x000DFBE4 File Offset: 0x000DDDE4
	// (set) Token: 0x060061B3 RID: 25011 RVA: 0x000DFBEC File Offset: 0x000DDDEC
	public ushort CircuitID { get; private set; }

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x060061B4 RID: 25012 RVA: 0x000DFBF5 File Offset: 0x000DDDF5
	// (set) Token: 0x060061B5 RID: 25013 RVA: 0x000DFBFD File Offset: 0x000DDDFD
	public float BaseWattageRating
	{
		get
		{
			return this._BaseWattageRating;
		}
		set
		{
			this._BaseWattageRating = value;
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x060061B6 RID: 25014 RVA: 0x000DFC06 File Offset: 0x000DDE06
	public float WattsUsed
	{
		get
		{
			if (this.operational.IsActive)
			{
				return this.BaseWattageRating;
			}
			return 0f;
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x060061B7 RID: 25015 RVA: 0x000DFC21 File Offset: 0x000DDE21
	public float WattsNeededWhenActive
	{
		get
		{
			return this.building.Def.EnergyConsumptionWhenActive;
		}
	}

	// Token: 0x060061B8 RID: 25016 RVA: 0x000DFC33 File Offset: 0x000DDE33
	protected override void OnPrefabInit()
	{
		this.CircuitID = ushort.MaxValue;
		this.IsPowered = false;
		this.BaseWattageRating = this.building.Def.EnergyConsumptionWhenActive;
	}

	// Token: 0x060061B9 RID: 25017 RVA: 0x002B3EBC File Offset: 0x002B20BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.EnergyConsumers.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddEnergyConsumer(this);
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x000DFC5D File Offset: 0x000DDE5D
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveEnergyConsumer(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.EnergyConsumers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060061BB RID: 25019 RVA: 0x000DFC91 File Offset: 0x000DDE91
	public virtual void EnergySim200ms(float dt)
	{
		this.CircuitID = Game.Instance.circuitManager.GetCircuitID(this);
		if (!this.IsConnected)
		{
			this.IsPowered = false;
		}
		this.circuitOverloadTime = Mathf.Max(0f, this.circuitOverloadTime - dt);
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x002B3F10 File Offset: 0x002B2110
	public virtual void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.IsPowered = false;
			return;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.IsPowered && base.GetComponent<Battery>() == null)
			{
				this.IsPowered = false;
				this.circuitOverloadTime = 6f;
				this.PlayCircuitSound("overdraw");
				return;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (!this.IsPowered && this.circuitOverloadTime <= 0f)
			{
				this.IsPowered = true;
				this.PlayCircuitSound("powered");
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060061BD RID: 25021 RVA: 0x002B3F94 File Offset: 0x002B2194
	protected void PlayCircuitSound(string state)
	{
		EventReference event_ref;
		if (state == "powered")
		{
			event_ref = Sounds.Instance.BuildingPowerOnMigrated;
		}
		else if (state == "overdraw")
		{
			event_ref = Sounds.Instance.ElectricGridOverloadMigrated;
		}
		else
		{
			event_ref = default(EventReference);
			global::Debug.Log("Invalid state for sound in EnergyConsumer.");
		}
		if (!CameraController.Instance.IsAudibleSound(base.transform.GetPosition()))
		{
			return;
		}
		float num;
		if (!this.lastTimeSoundPlayed.TryGetValue(state, out num))
		{
			num = 0f;
		}
		float value = (Time.time - num) / this.soundDecayTime;
		Vector3 position = base.transform.GetPosition();
		position.z = 0f;
		FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(event_ref, CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		instance.setParameterByName("timeSinceLast", value, false);
		KFMOD.EndOneShot(instance);
		this.lastTimeSoundPlayed[state] = Time.time;
	}

	// Token: 0x060061BE RID: 25022 RVA: 0x000AD332 File Offset: 0x000AB532
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x04004584 RID: 17796
	[MyCmpReq]
	private Building building;

	// Token: 0x04004585 RID: 17797
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04004586 RID: 17798
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04004587 RID: 17799
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x04004589 RID: 17801
	[Serialize]
	protected float circuitOverloadTime;

	// Token: 0x0400458A RID: 17802
	public static readonly Operational.Flag PoweredFlag = new Operational.Flag("powered", Operational.Flag.Type.Requirement);

	// Token: 0x0400458B RID: 17803
	private Dictionary<string, float> lastTimeSoundPlayed = new Dictionary<string, float>();

	// Token: 0x0400458C RID: 17804
	private float soundDecayTime = 10f;

	// Token: 0x04004590 RID: 17808
	private float _BaseWattageRating;
}
